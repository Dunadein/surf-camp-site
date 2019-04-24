using SurfLevel.Contracts.Models.DatabaseObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SurfLevel.Contracts.Interfaces.Repositories
{
    public interface IBookingRepository
    {
        Task<List<Order>> GetBookingsAsync(DateTime? startFrom = null);

        Task<List<Order>> GetBookingsInPeriodAsync(DateTime periodStart, DateTime? periodEnd = null);
    }
}
