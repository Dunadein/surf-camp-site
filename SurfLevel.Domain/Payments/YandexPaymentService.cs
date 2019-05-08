using MediatR;
using SurfLevel.Contracts.Interfaces.Services;
using SurfLevel.Contracts.Models.DTO;
using SurfLevel.Domain.Events;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using YandexPaymentProvider.DTO;
using YandexPaymentProvider.Interfaces;

namespace SurfLevel.Domain.Payments
{
    public class YandexPaymentService : IPaymentService
    {
        private const int EURO_CODE = 978;
        private const decimal SURCHARGE = 1.03m;
        private const decimal MAX_PAY_AMOUNT = 15000m;

        private readonly IYandexPaymentProvider _provider;
        private readonly IMediator _bus;
        private readonly HttpClient _httpClient;

        public YandexPaymentService(IYandexPaymentProvider paymentProvider, HttpClient httpClient, IMediator bus)
        {
            _provider = paymentProvider;
            _bus = bus;
            _httpClient = httpClient;
        }

        private async Task<string> MakeCall(string url, HttpContent content)
        {
            using (var response = await _httpClient.PostAsync(url, content))
            {
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
        }

        private async Task<decimal> GetCourse()
        {
            var envelop = $@"<?xml version=""1.0"" encoding=""utf-8""?>
                <soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
                    <soap:Body>
                    <GetCursOnDate xmlns=""http://web.cbr.ru/"">
                        <On_date>{DateTime.Now.ToString("yyyy-MM-dd")}</On_date>
                    </GetCursOnDate>
                    </soap:Body>
                </soap:Envelope>";

            var httpContent = new StringContent(envelop, Encoding.UTF8, "text/xml");

            var response = await MakeCall("", httpContent);

            var result = XDocument.Parse(response).Descendants("Vcode")
                .FirstOrDefault(p => int.TryParse(p.Value, out int code) && code == EURO_CODE)?.PreviousNode;

            if (result == null)
                throw new ArgumentNullException("Can't retrieve the accurate course.");

            return decimal.Parse(XElement.Parse(result.ToString()).Value.Replace('.', ',')) * SURCHARGE;
        }

        public async Task<string> GetPaymentURL(int orderId, string orderLabel, decimal amount, string orderIdURL)
        {
            var course = await GetCourse();

            var amountToPay = amount * course;

            var result = await _provider.GetRequestId(orderLabel, amountToPay);

            if (!string.IsNullOrEmpty(result?.RequestId))
            { 
                await _bus.Publish(new PaymentInitialization()
                {
                    AmountToPay = amountToPay,
                    Amount = amountToPay - result.Fee,
                    OrderId = orderId,
                    RequestId = result.RequestId,
                    EuroAmount = amount
                });

                return await _provider.GetRedirectUrl(result.RequestId, orderIdURL);
            }

            return null;
        }

        public async Task<decimal> GetConvertedAmount(decimal euroAmount)
        {
            var course = await GetCourse();

            return euroAmount * course;
        }

        public bool IsOperableAmount(decimal amount)
        {
            return amount <= MAX_PAY_AMOUNT;
        }

        public async Task<PaymentStatus> GetPaymentStatus(string requestId)
        {
            var result = await _provider.CheckPaymentRetryPeriod(requestId);

            if (result.Status == YandexStatus.Refused)
                return new PaymentStatus() { Status = Status.Refused };

            if (result.Status == YandexStatus.NeedRetry)
                return new PaymentStatus() { Status = Status.Unknown, RetryIn = result.RetrySeconds };

            if (result.Status == YandexStatus.Success)
            {
                await _bus.Publish(new PaymentSucceeded()
                {
                    RequestID = requestId
                });

                return new PaymentStatus() { Status = Status.Success };
            }

            throw new ArgumentException($"Can't handle {result.Status} status.");
        }
    }
}
