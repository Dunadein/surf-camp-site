using SurfLevel.Contracts.Models.DTO;
using SurfLevel.Domain.ViewModels.Order;
using System.Threading.Tasks;

namespace SurfLevel.Domain.Providers.Interfaces
{
    public interface IOrderProvider
    {
        Task<ViewOrder> GetOrder(string orderId);

        Task<string> GetPaymentUrl(string hash, PrepayType type, int? serviceId = null);

        Task UpdateGuest(string hash, int number, string name, string secondName);
    }
}
