using Moq;
using NUnit.Framework;
using SurfLevel.Contracts.Interfaces.Repositories;
using SurfLevel.Contracts.Models.DatabaseObjects;
using SurfLevel.Domain.Services;
using SurfLevel.Domain.Test.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurfLevel.Domain.Test
{
    public class BookingsServiceTest  : AccommodationSetupWithOrderStub
    {
        [Test]
        public async Task Assure_All_Rooms_Are_Booked_And_Counting_Correct()
        {
            var bookingRepo = new Mock<IBookingRepository>();
            // все занято
            bookingRepo.Setup(p => p.GetBookingsInPeriodAsync(It.IsAny<DateTime>(), It.IsAny<DateTime?>())).ReturnsAsync(
                new List<Order>()
                {
                    CreateTypicalOrder(DateTime.Now.AddDays(5), DateTime.Now.AddDays(12), 2, 1, OrderStatus.Confirmed),
                    CreateTypicalOrder(DateTime.Now.AddDays(5), DateTime.Now.AddDays(7), 1, 3, OrderStatus.Confirmed),
                    CreateTypicalOrder(DateTime.Now.AddDays(8), DateTime.Now.AddDays(20), 1, 3, OrderStatus.Confirmed),
                    CreateTypicalOrder(DateTime.Now.AddDays(8), null, 6, null, OrderStatus.Confirmed),
                    CreateTypicalOrder(DateTime.Now.AddDays(12), DateTime.Now.AddDays(14), 2, 2, OrderStatus.Confirmed, 3),
                    CreateTypicalOrder(DateTime.Now.AddDays(7), DateTime.Now.AddDays(14), 2, 2, OrderStatus.Annulated, 3),
                }
            );

            var bookings = new BookingsService(bookingRepo.Object);

            var result = await bookings.GetBookingsWithDates(DateTime.Now.AddDays(8), DateTime.Now.AddDays(15));

            var accommodations = await _accommodationRepository.GetAccommodationsAsync();
            var maxPax = accommodations.Where(p => p.IsEnabled).SelectMany(p => p.Rooms).Sum(t =>
                t.Prices.Max(p => p.Accommodation.Сapacity)
            );

            // вернулось 16 дней
            Assert.AreEqual(16, result.Count);

            // без проживания отделимы и не участвуют в учете загрузки комнат
            Assert.GreaterOrEqual(maxPax, result.Max(p => p.Groupping.Where(g => g.RoomKey.HasValue).Sum(g => g.Pax)));

            // в один из дней все жилье занято полностью
            Assert.AreEqual(1, result.Where(r => r.Groupping.Where(p => p.RoomKey.HasValue).Sum(p => p.Pax) == maxPax).Count());
        }

        [Test]
        public async Task Assure_Request_Dont_Counted_As_Booking()
        {
            var bookingRepo = new Mock<IBookingRepository>();
            // все занято и запрос на овербукинг
            bookingRepo.Setup(p => p.GetBookingsInPeriodAsync(It.IsAny<DateTime>(), It.IsAny<DateTime?>())).ReturnsAsync(
                new List<Order>()
                {
                    CreateTypicalOrder(DateTime.Now.AddDays(5), DateTime.Now.AddDays(12), 2, 1, OrderStatus.Confirmed),
                    CreateTypicalOrder(DateTime.Now.AddDays(5), DateTime.Now.AddDays(7), 1, 3, OrderStatus.Confirmed),
                    CreateTypicalOrder(DateTime.Now.AddDays(8), DateTime.Now.AddDays(20), 1, 3, OrderStatus.Confirmed),
                    CreateTypicalOrder(DateTime.Now.AddDays(12), DateTime.Now.AddDays(14), 2, 2, OrderStatus.Confirmed),
                    CreateTypicalOrder(DateTime.Now.AddDays(7), DateTime.Now.AddDays(14), 2, 2, OrderStatus.Request, 3),
                }
            );

            var bookings = new BookingsService(bookingRepo.Object);

            var result = await bookings.GetBookingsWithDates(DateTime.Now.AddDays(8), DateTime.Now.AddDays(15));

            var accommodations = await _accommodationRepository.GetAccommodationsAsync();
            var maxPax = accommodations.Where(p => p.IsEnabled).SelectMany(p => p.Rooms).Sum(t =>
                t.Prices.Max(p => p.Accommodation.Сapacity)
            );

            // ни в один из дней нет максимальной загрузки (1 место свободно), хотя в один из дней есть овербукинг под запрос
            Assert.GreaterOrEqual(maxPax, result.Max(p => p.Groupping.Where(g => g.RoomKey.HasValue).Sum(g => g.Pax)));
        }
    }
}
