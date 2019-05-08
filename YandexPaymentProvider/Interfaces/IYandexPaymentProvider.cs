using System.Threading.Tasks;
using YandexPaymentProvider.DTO;

namespace YandexPaymentProvider.Interfaces
{
    public interface IYandexPaymentProvider
    {
        Task<PreparePaymentResult> GetRequestId(string orderLabel, decimal amount);

        Task<string> GetRedirectUrl(string requestId, string orderIdURL);

        Task<PaymentStatusResult> CheckPaymentRetryPeriod(string requestId);
    }
}
