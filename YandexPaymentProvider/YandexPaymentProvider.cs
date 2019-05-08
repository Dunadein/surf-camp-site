using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using YandexPaymentProvider.DTO;
using YandexPaymentProvider.Interfaces;
using YandexPaymentProvider.Options;
using YandexPaymentProvider.Response;

namespace YandexPaymentProvider
{
    public class PaymentProvider : IYandexPaymentProvider
    {
        private readonly YandexPaymentOptions _options;
        private readonly IYandexProviderRepository _repository;
        private readonly HttpClient _httpClient;

        private static string InstanceID = null;

        public PaymentProvider(IOptions<YandexPaymentOptions> options,
            HttpClient httpClient,
            IYandexProviderRepository repository)
        {
            _options = options.Value;
            _repository = repository;
            _httpClient = httpClient;
        }

        private class KeyValueList : List<KeyValuePair<string, string>>
        {
            public void Add(string key, string value)
            {
                Add(new KeyValuePair<string, string>(key, value));
            }
        }

        private async Task<TResult> MakeCall<TResult>(string url, KeyValueList content)
        {
            using (var response = await _httpClient.PostAsync(url, new FormUrlEncodedContent(content)))
            {
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsStringAsync();

                return JsonConvert.DeserializeObject<TResult>(result);
            }
        }

        private async Task<string> RequestInstanceId(string clientId)
        {  
            var instanceIdResult = await MakeCall<InstanceIdResult>("instance-id",
                new KeyValueList { { "client_id", clientId } }
            );

            if (instanceIdResult.Status == ResponseStatus.Success)
            {
                await _repository.SaveInstanceId(instanceIdResult.InstanceId);

                return instanceIdResult.InstanceId;
            }

            throw new ArgumentException($"Failed to get InstanceID. Operation was finished with {instanceIdResult.Status} code.");
        }

        private async Task<string> GetInstanceId(string clientId)
        {
            var fromBD = await _repository.GetInstanceId();

            if (!string.IsNullOrEmpty(fromBD))
                return fromBD;

            return await RequestInstanceId(clientId);
        }

        public async Task<PreparePaymentResult> GetRequestId(string orderLabel, decimal amount)
        {
            InstanceID = InstanceID ?? await GetInstanceId(_options.ClientID);

            var @params = new KeyValueList
            {
                { "pattern_id", "p2p" },
                { "to", _options.AccountNumber },
                { "amount", amount.ToString("F2").Replace(',', '.') },
                { "message", $"Предоплата по заказу {orderLabel}" },
                { "instance_id", InstanceID },
                { "label", $"Предоплата по заказу {orderLabel}" },
            };

            var paymentRequest = await MakeCall<RequestPaymentResult>("request-external-payment", @params);

            if (paymentRequest.Status == ResponseStatus.Success)
            {
                return new PreparePaymentResult()
                {
                    RequestId = paymentRequest.RequestID,
                    Fee = paymentRequest.Fee.Commission
                };
            }

            return null;
        }

        public async Task<string> GetRedirectUrl(string requestId, string orderIdURL)
        {
            InstanceID = InstanceID ?? await GetInstanceId(_options.ClientID);

            var @params = new KeyValueList
            {
                { "request_id", requestId },
                { "instance_id", InstanceID },
                { "ext_auth_success_uri", string.Format(_options.CallBackUrlTemplate, orderIdURL) },
                { "ext_auth_fail_uri" , string.Format(_options.CallBackUrlTemplate, orderIdURL) }
            };

            var processResult = await MakeCall<ProcessPaymentResult>("process-external-payment", @params);

            if (processResult.Status == ResponseStatus.ExtAuthRequired)
            {
                return $@"{processResult.BaseUri}?{string.Join("&",
                    processResult.Parameters.Select(p => string.Format("{0}={1}", p.Key, p.Value)))}";
            }

            return null;
        }

        public async Task<PaymentStatusResult> CheckPaymentRetryPeriod(string requestId)
        {
            InstanceID = InstanceID ?? await GetInstanceId(_options.ClientID);

            var @params = new KeyValueList
            {
                { "request_id", requestId },
                { "instance_id", InstanceID },
                { "ext_auth_success_uri", "/" },
                { "ext_auth_fail_uri" , "/" }
            };

            var processPaymentResult = await MakeCall<StatusCheckResult>("process-external-payment", @params);

            if (processPaymentResult.Status == ResponseStatus.Success)
            {
                return new PaymentStatusResult()
                {
                    InvoiceId = processPaymentResult.InvoiceId,
                    Status = YandexStatus.Success
                };
            }

            if (processPaymentResult.Status == ResponseStatus.Refused)
                return new PaymentStatusResult() { Status = YandexStatus.Refused };

            return new PaymentStatusResult()
            {
                Status = YandexStatus.NeedRetry,
                RetrySeconds = (processPaymentResult.NextRetry > 0 ? processPaymentResult.NextRetry : 5000) / 1000
            };
        }
    }
}
