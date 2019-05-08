using Moq;
using NUnit.Framework;
using System;
using System.Net.Http;
using YandexPaymentProvider.DTO;
using YandexPaymentProvider.Interfaces;
using YandexPaymentProvider.Options;
using MSOpt = Microsoft.Extensions.Options.Options;

namespace YandexPaymentProvider.Test
{
    public class YandexPaymentProviderTest
    {
        private IYandexPaymentProvider _provider;

        private readonly static HttpClient _httpClient = new HttpClient()
        {
            BaseAddress = new Uri("https://money.yandex.ru/api/")
        };

        [SetUp]
        public void Setup()
        {
            var options = MSOpt.Create(new YandexPaymentOptions()
            {
                AccountNumber = "<account>",
                ClientID = "<clientId>",
                CallBackUrlTemplate = "http://surflevel.ru/booking/success?id={0}"
            });

            var repo = new Mock<IYandexProviderRepository>();

            repo.Setup(p => p.GetInstanceId()).ReturnsAsync("<instanceId>");

            _provider = new PaymentProvider(options, _httpClient, repo.Object);
        }

        //[Test]
        public void Assure_Transaction_Was_Successful()
        {
            var task = _provider.CheckPaymentRetryPeriod("<requestId>");

            Assert.DoesNotThrow(() => task.Wait());

            Assert.AreEqual(YandexStatus.Success, task.Result.Status);
        }

        //[Test]
        public void Assure_Provider_Return_Id_And_Redirect()
        {
            var requestIdTask = _provider.GetRequestId("any", 5);

            var requestUrlTask = _provider.GetRedirectUrl(requestIdTask.Result?.RequestId, "any");

            Assert.DoesNotThrow(() => requestIdTask.Wait());

            Assert.False(string.IsNullOrEmpty(requestIdTask.Result?.RequestId));

            Assert.DoesNotThrow(() => requestUrlTask.Wait());

            Assert.False(string.IsNullOrEmpty(requestUrlTask.Result));
        }
    }
}