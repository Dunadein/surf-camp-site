using SurfLevel.Contracts.Interfaces.Repositories;
using SurfLevel.Contracts.Interfaces.Services;
using SurfLevel.Domain.Providers.Interfaces;
using SurfLevel.Domain.ViewModels.Calendar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurfLevel.Domain.Providers
{
    public class CalendarProvider : ICalendarProvider
    {
        private readonly IAccommodationRepository _accommodationRepository;
        private readonly IBookingsService _bookings;

        public CalendarProvider(IAccommodationRepository accommodationRepository,
            IBookingsService bookingsService)
        {
            _bookings = bookingsService;
            _accommodationRepository = accommodationRepository;
        }

        public async Task<List<CalendarDate>> GetCalendarDates()
        {
            var accommodations = await _accommodationRepository.GetAccommodationsAsync(p => p.IsEnabled);
            // максимальная вместимость жилья
            var maxPax = accommodations.SelectMany(p => p.Rooms).Sum(t => 
                t.Prices.Max(p => p.Accommodation.Capacity)
            );

            // получаем загрузку по броням
            var bookings = await _bookings.GetBookingsWithDates(DateTime.UtcNow.AddDays(1));

            // и и считаем полную загрузку по датам
            // причем те кто бронировался без жилья не должны занимать места
            var loading = bookings.Select(p => new
            {
                BookingDate = p.Date,
                TotalPax = p.Groupping.Sum(group => group.RoomKey.HasValue ? group.Pax : group.Pax * 0.1)
            });

            // составляем календарь с учетом загрузки
            var result = GetDaysRange(DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddYears(1)).Select(p => new CalendarDate()
            {
                Date = p,
                LoadingState = GetLoadingStateByPax
                (
                    loading.FirstOrDefault(t => t.BookingDate.Date.Equals(p))?.TotalPax ?? 0,
                    maxPax
                )
            });

            return result.ToList();
        }

        private LoadingState GetLoadingStateByPax(double pax, int maxPax)
        {
            switch (pax)
            {
                case double x when Math.Floor(x) >= maxPax:
                    return LoadingState.BookedOut;
                case double x when Math.Floor(x) > maxPax - 4: // место под среднестатистическую семью
                    return LoadingState.Few;
                case double x when x > 0:
                    return LoadingState.Ok;
                default: return LoadingState.Many;
            }
        }

        private IEnumerable<DateTime> GetDaysRange(DateTime from, DateTime till)
        {
            return Enumerable.Range(0, (till.Date - from.Date).Days + 1).Select(day => from.Date.AddDays(day).Date).ToList();
        }
    }
}
