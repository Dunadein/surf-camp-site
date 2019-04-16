using SurfLevel.Contracts.Interfaces.Repositories;
using SurfLevel.Contracts.Interfaces.Services;
using SurfLevel.Contracts.Models.DatabaseObjects;
using SurfLevel.Contracts.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurfLevel.Domain.Services
{
    public class CalendarProvider : ICalendarProvider
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IAccommodationRepository _accommodationRepository;

        public CalendarProvider(IBookingRepository bookingRepository, IAccommodationRepository accommodationRepository)
        {
            _bookingRepository = bookingRepository;
            _accommodationRepository = accommodationRepository;
        }

        public async Task<IEnumerable<CalendarDate>> GetCalendarDates()
        {
            var accommodations = await _accommodationRepository.GetAccommodationsAsync();
            // максимальная вместимость жилья
            var maxPax = accommodations.Where(p => p.IsEnabled).SelectMany(p => p.Rooms).Sum(t => 
                t.Accommodations.Max(p => p.Сapacity)
            );

            var orders = await _bookingRepository.GetBookingsAsync(DateTime.UtcNow);
            // составляем массив с продолжительностями и суммарной загрузкой по брони
            var bookingsWithRange = orders.Where(p => p.Status != OrderStatus.Annulated).Select(p => new
            {
                Date = GetDaysRange(p.DateFrom, p.DateTill ?? p.DateFrom.AddDays(p.Guests.Max(g => g.Duration) - 1)),
                Pax = p.Guests.GroupBy(t => t.AccommodationPriceId).Sum(g => g.Key.HasValue ?
                    Math.Max(g.Count(), g.Max(t => t.AccommodationPrice.Accommodation.Сapacity))
                    : 0.1 // кто бронировал пакет только с уроками не должны занимать паксов, но они все равно должны влиять на календарь
                )
            });
            // и делаем календарь загрузки
            var loading = bookingsWithRange.SelectMany(p => p.Date.Select(t => new { Date = t, p.Pax }))
                .GroupBy(d => d.Date).Select(d => new { BookingDate = d.Key, TotalPax = d.Sum(p => p.Pax) });
            // составляем календарь с учетом загрузки
            var result = GetDaysRange(DateTime.UtcNow.AddDays(1), DateTime.UtcNow.AddYears(1)).Select(p => new CalendarDate()
            {
                Date = p,
                LoadingState = GetLoadingStateByPax
                (
                    loading.FirstOrDefault(t => t.BookingDate.Date.Equals(p.Date))?.TotalPax ?? 0,
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
            return Enumerable.Range(0, (till.Date - from.Date).Days + 1).Select(day => from.Date.AddDays(day)).ToList();
        }
    }
}
