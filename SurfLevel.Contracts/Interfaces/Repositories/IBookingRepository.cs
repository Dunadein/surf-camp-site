using SurfLevel.Contracts.Models.DatabaseObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SurfLevel.Contracts.Interfaces.Repositories
{
    public interface IBookingRepository
    {
        Task<IEnumerable<Order>> GetBookingsAsync(DateTime? startFrom = null);
    }
}
