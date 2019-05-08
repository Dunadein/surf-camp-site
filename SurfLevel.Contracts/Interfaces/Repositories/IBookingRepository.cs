using SurfLevel.Contracts.Models.DatabaseObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SurfLevel.Contracts.Interfaces.Repositories
{
    public interface IBookingRepository
    {
        Task<List<Order>> GetBookingsByConditionAsync(Func<Order, bool> condition = null);

        Task<int> CreateBookingAsync(Order order);

        Task<Order> GetBookingByConditionAsync(Func<Order, bool> condition);

        Task UpdateTouristAsync(int id, string name, string secondName);

        Task UpdateOrderAsync(int id, Action<Order> action);
    }
}
