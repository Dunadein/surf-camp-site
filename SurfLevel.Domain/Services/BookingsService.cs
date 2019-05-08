using SurfLevel.Contracts.Interfaces.Repositories;
using SurfLevel.Contracts.Interfaces.Services;
using SurfLevel.Contracts.Models.DatabaseObjects;
using SurfLevel.Contracts.Models.DTO;
using SurfLevel.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SurfLevel.Domain.Fetching.FetchStrategy;

namespace SurfLevel.Domain.Services
{
    public class BookingsService : IBookingsService
    {
        private readonly IBookingRepository _bookings;

        public BookingsService(IBookingRepository bookingRepository)
        {
            _bookings = bookingRepository;
        }

        public async Task<List<DateWithGroupingByRoom>> GetBookingsWithDates(DateTime periodStart, DateTime? periodEnd = null, bool includeRequested = false)
        {
            var bookings = await _bookings.GetBookingsByConditionAsync(InPeriod(periodStart, periodEnd));
            // что занято уже
            // иногда нужно получать только реально занимающие слоты, иногда все брони
            var occupied = bookings.Where(p => !(includeRequested ? p.Status.In(OrderStatus.Annulated) : p.Status.In(OrderStatus.Annulated, OrderStatus.Request)))
               .Select(p => new
               {
                   Dates = GetDaysRange(p.DateFrom, p.DateTill ?? p.DateFrom.AddDays(p.Services.Max(g => g.ServiceDays ?? 1) - 1)),
                   GrouppingByOrder = p.Services.GroupBy(t => t.AccommodationPrice?.RoomId).Select(g => new 
                   {
                       RoomKey = g.Key,
                       Pax = g.Key.HasValue ? Math.Max(g.Count(), g.Max(t => t.AccommodationPrice.Accommodation.Сapacity)) : g.Count()
                   })
               })
               .SelectMany(p => p.Dates.Select(d => new { Date = d, p.GrouppingByOrder })).GroupBy(p => p.Date).Select(p => new DateWithGroupingByRoom()
               {
                   Date = p.Key,
                   Groupping = p.SelectMany(a => a.GrouppingByOrder).GroupBy(a => a.RoomKey).Select(a => new GroupTypePax()
                   {
                       RoomKey = a.Key,
                       Pax = a.Sum(group => group.Pax)
                   }).ToList()
               });

            return occupied.ToList();
        }

        private IEnumerable<DateTime> GetDaysRange(DateTime from, DateTime till)
        {
            return Enumerable.Range(0, (till.Date - from.Date).Days + 1).Select(day => from.Date.AddDays(day).Date);
        }
    }
}
