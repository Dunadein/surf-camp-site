using SurfLevel.Contracts.Models.DatabaseObjects;
using System;
using System.Threading.Tasks;

namespace SurfLevel.Contracts.Interfaces.Repositories
{
    public interface IPaymentRepository
    {
        Task SaveRowAsync(PayLog row);

        Task<PayLog> GetRowByConditionAsync(Func<PayLog, bool> condition);

        Task UpdateRow(int id, Action<PayLog> action);
    }
}
