using SurfLevel.Contracts.Models.DTO;
using System.Threading.Tasks;

namespace SurfLevel.Contracts.Interfaces.Services
{
    public interface IPaymentService
    {
        Task<string> GetPaymentURL(int orderId, string orderLabel, decimal amount, string orderIdURL);

        bool IsOperableAmount(decimal amount);

        Task<decimal> GetConvertedAmount(decimal euroAmount);

        Task<PaymentStatus> GetPaymentStatus(string requestId);
    }
}
