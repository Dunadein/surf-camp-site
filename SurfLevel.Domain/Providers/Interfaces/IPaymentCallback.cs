using SurfLevel.Contracts.Models.DTO;
using System.Threading.Tasks;

namespace SurfLevel.Domain.Providers.Interfaces
{
    public interface IPaymentCallback
    {
        Task<PaymentStatus> CheckPayment(string requestId);
    }
}
