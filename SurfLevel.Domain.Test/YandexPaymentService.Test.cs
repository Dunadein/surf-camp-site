using MediatR;
using Moq;
using NUnit.Framework;
using SurfLevel.Contracts.Interfaces.Services;
using SurfLevel.Domain.Payments;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using YandexPaymentProvider.DTO;
using YandexPaymentProvider.Interfaces;

namespace SurfLevel.Domain.Test
{
    public class YandexPaymentServiceTest
    {
        private IPaymentService _paymentService;

        private readonly static HttpClient _httpClient = new HttpClient()
        {
            BaseAddress = new Uri("https://www.cbr.ru/DailyInfoWebServ/DailyInfo.asmx")
        };

        [SetUp]
        public void SetUp()
        {
            var bus = new Mock<IMediator>();

            var provider = new Mock<IYandexPaymentProvider>();

            provider.Setup(p => p.GetRequestId(It.IsAny<string>(), It.IsAny<decimal>())).ReturnsAsync(new PreparePaymentResult()
            {
                RequestId = "anystring"
            });

            provider.Setup(p => p.GetRedirectUrl(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("anyotherstring");

            _paymentService = new YandexPaymentService(provider.Object, _httpClient, bus.Object);
        }

        [Test]
        public async Task Assure_Service_Returns_Course_Value()
        {
            var result = await _paymentService.GetConvertedAmount(1);

            Assert.Greater(result, 1);
        }
    }
}
