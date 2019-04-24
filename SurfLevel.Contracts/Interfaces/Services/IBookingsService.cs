using SurfLevel.Contracts.Models.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SurfLevel.Contracts.Interfaces.Services
{
    public interface IBookingsService
    {
        Task<List<DateWithGroupingByRoom>> GetBookingsWithDates(DateTime periodStart, DateTime? periodEnd = null, bool includeRequested = true);
    }
}
