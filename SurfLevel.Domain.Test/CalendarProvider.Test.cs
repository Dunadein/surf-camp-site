using Moq;
using NUnit.Framework;
using SurfLevel.Contracts.Interfaces.Repositories;
using SurfLevel.Contracts.Models.DatabaseObjects;
using SurfLevel.Contracts.Models.ViewModels.Calendar;
using SurfLevel.Domain.Services;
using SurfLevel.Domain.Test.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurfLevel.Domain.Test
{
    public class CalendarProviderTest : AccommodationSetupWithOrderStub
    {
        [Test]
        public async Task Assure_All_Orders_Calculate_Correctly()
        {
            var bookingRepository = new Mock<IBookingRepository>();

            // сетапим на пиковую загрузку в 6 мест в один из дней (через неделю от теста)
            bookingRepository.Setup(p => p.GetBookingsInPeriodAsync(It.IsAny<DateTime>(), It.IsAny<DateTime?>())).ReturnsAsync(
                new List<Order>()
                {
                    CreateTypicalOrder(DateTime.Now.AddDays(5), DateTime.Now.AddDays(12), 2, 1, OrderStatus.Confirmed),
                    CreateTypicalOrder(DateTime.Now.AddDays(5), DateTime.Now.AddDays(7), 1, 3, OrderStatus.Confirmed),
                    CreateTypicalOrder(DateTime.Now.AddDays(7), DateTime.Now.AddDays(14), 2, 2, OrderStatus.Confirmed, 3),
                    CreateTypicalOrder(DateTime.Now.AddDays(7), DateTime.Now.AddDays(14), 2, 2, OrderStatus.Annulated, 3),
                    CreateTypicalOrder(DateTime.Now.AddDays(15), DateTime.Now.AddDays(20), 1, 1, OrderStatus.Payed, 2),
                    CreateTypicalOrder(DateTime.Now.AddDays(20), DateTime.Now.AddDays(24), 1, 2, OrderStatus.Request, 3),
                    CreateTypicalOrder(DateTime.Now.AddDays(20), null, 2, null, OrderStatus.Request), // не должен учитываться в загрузке,
                    CreateTypicalOrder(DateTime.Now.AddDays(30), null, 2, null, OrderStatus.Request) // должен учитываться в загрузке
                }
            );
            var bookings = new BookingsService(bookingRepository.Object);

            var provider = new CalendarProvider(_accommodationRepository, bookings);

            var result = await provider.GetCalendarDates();

            Assert.NotNull(result);
            // есть статус Нет мест
            Assert.IsTrue(result.Any(p => p.LoadingState == LoadingState.BookedOut));
            // и он такой один, серьезно, тут все засетапено так чтобы он был один.
            Assert.LessOrEqual(result.Count(p => p.LoadingState == LoadingState.BookedOut), 1);
            // есть статус Места есть (где одна бронь только)
            Assert.IsTrue(result.Any(p => p.LoadingState == LoadingState.Ok));
            // есть много свободных мест за пределами пика
            Assert.Greater(result.Count(p => p.LoadingState == LoadingState.Many), 300);
            // учитываются брони без проживания (когда нет в те же даты броней с проживанием)
            Assert.IsTrue(result.Where(p => p.Date.CompareTo(DateTime.Now.AddDays(30)) >= 0).Any(p => p.LoadingState == LoadingState.Ok));
        }
    }
}