using System.Threading.Tasks;

namespace YandexPaymentProvider.Interfaces
{
    public interface IYandexProviderRepository
    {
        Task SaveInstanceId(string instanceId);

        Task<string> GetInstanceId();
    }
}
