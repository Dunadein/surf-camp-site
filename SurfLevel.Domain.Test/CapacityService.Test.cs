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
    public class CapacityServiceTest : AccommodationSetupWithOrderStub
    {
        [Test]
        public async Task Assure_Service_Return_Nothing()
        {
            var bookingRepo = new Mock<IBookingRepository>();
            // в один из дней мест нет
            bookingRepo.Setup(p => p.GetBookingsByConditionAsync(It.IsAny<Func<Order, bool>>())).ReturnsAsync(
                new List<Order>()
                {
                    CreateTypicalOrder(DateTime.Now.AddDays(5), DateTime.Now.AddDays(12), 2, 1, OrderStatus.Confirmed),
                    CreateTypicalOrder(DateTime.Now.AddDays(5), DateTime.Now.AddDays(7), 1, 3, OrderStatus.Confirmed),
                    CreateTypicalOrder(DateTime.Now.AddDays(7), DateTime.Now.AddDays(14), 2, 2, OrderStatus.Confirmed, 3),
                    CreateTypicalOrder(DateTime.Now.AddDays(7), DateTime.Now.AddDays(14), 2, 2, OrderStatus.Annulated, 3),
                }
            );
            
            var bookings = new BookingsService(bookingRepo.Object);

            var service = new CapacityService(_accommodationRepository, bookings);

            var result = await service.FindAvailableAccommodation(DateTime.Now.AddDays(4), DateTime.Now.AddDays(10), 2);

            // ничего не доступно
            Assert.GreaterOrEqual(0, result.Count);
        }

        [Test]
        public async Task Assure_Service_Return_Only_One_Accommodations()
        {
            var bookingRepo = new Mock<IBookingRepository>();
            // есть доступная комната на двоих
            bookingRepo.Setup(p => p.GetBookingsByConditionAsync(It.IsAny<Func<Order, bool>>())).ReturnsAsync(
                new List<Order>()
                {
                    CreateTypicalOrder(DateTime.Now.AddDays(4), DateTime.Now.AddDays(6), 2, 2, OrderStatus.Confirmed, 3),
                    CreateTypicalOrder(DateTime.Now.AddDays(5), DateTime.Now.AddDays(7), 1, 3, OrderStatus.Confirmed),
                    CreateTypicalOrder(DateTime.Now.AddDays(7), DateTime.Now.AddDays(14), 2, 2, OrderStatus.Confirmed, 3),
                    CreateTypicalOrder(DateTime.Now.AddDays(7), DateTime.Now.AddDays(14), 2, 2, OrderStatus.Annulated, 3),
                }
            );

            var bookings = new BookingsService(bookingRepo.Object);

            var service = new CapacityService(_accommodationRepository, bookings);

            var result = await service.FindAvailableAccommodation(DateTime.Now.AddDays(4), DateTime.Now.AddDays(10), 2);

            // доступна только одна комната
            Assert.AreEqual(1, result.Count);

            // и в ней можно разместить всех (только одна цена)
            Assert.AreEqual(1, result.SelectMany(p => p.Prices).Count());
        }

        [Test]
        public async Task Assure_Service_Return_Two_Rooms_With_Singles()
        {
            var bookingRepo = new Mock<IBookingRepository>();
            // есть 2 комнаты по 1 месту
            bookingRepo.Setup(p => p.GetBookingsByConditionAsync(It.IsAny<Func<Order, bool>>())).ReturnsAsync(
                new List<Order>()
                {
                    CreateTypicalOrder(DateTime.Now.AddDays(4), DateTime.Now.AddDays(6), 1, 2, OrderStatus.Confirmed),
                    CreateTypicalOrder(DateTime.Now.AddDays(7), DateTime.Now.AddDays(14), 2, 2, OrderStatus.Confirmed),
                    CreateTypicalOrder(DateTime.Now.AddDays(5), DateTime.Now.AddDays(7), 1, 3, OrderStatus.Confirmed),
                    CreateTypicalOrder(DateTime.Now.AddDays(6), DateTime.Now.AddDays(16), 1, 1, OrderStatus.Confirmed),                    
                    CreateTypicalOrder(DateTime.Now.AddDays(7), DateTime.Now.AddDays(14), 2, 2, OrderStatus.Annulated),
                }
            );

            var bookings = new BookingsService(bookingRepo.Object);

            var service = new CapacityService(_accommodationRepository, bookings);

            var result = await service.FindAvailableAccommodation(DateTime.Now.AddDays(4), DateTime.Now.AddDays(10), 2);

            // две комнаты
            Assert.AreEqual(2, result.Count);

            // две цены
            Assert.AreEqual(2, result.SelectMany(p => p.Prices).Count());

            // максимально на одного человека
            Assert.AreEqual(1, result.SelectMany(p => p.Prices).Max(p => p.Accommodation.Capacity));
        }

        [Test]
        public async Task Assure_Service_Return_Three_Rooms_With_Singles()
        {
            var bookingRepo = new Mock<IBookingRepository>();
            // есть 3 комнаты по 1 месту
            bookingRepo.Setup(p => p.GetBookingsByConditionAsync(It.IsAny<Func<Order, bool>>())).ReturnsAsync(
                new List<Order>()
                {
                    CreateTypicalOrder(DateTime.Now.AddDays(1), DateTime.Now.AddDays(3), 1, 3, OrderStatus.Confirmed),

                    CreateTypicalOrder(DateTime.Now.AddDays(4), DateTime.Now.AddDays(7), 2, 2, OrderStatus.Confirmed),
                    CreateTypicalOrder(DateTime.Now.AddDays(8), DateTime.Now.AddDays(20), 1, 2, OrderStatus.Confirmed, 2),
                    CreateTypicalOrder(DateTime.Now.AddDays(7), DateTime.Now.AddDays(14), 2, 2, OrderStatus.Request, 3),

                    CreateTypicalOrder(DateTime.Now.AddDays(4), DateTime.Now.AddDays(7), 1, 1, OrderStatus.Confirmed),
                    CreateTypicalOrder(DateTime.Now.AddDays(8), DateTime.Now.AddDays(17), 1, 1, OrderStatus.Confirmed)                   
                }
            );

            var bookings = new BookingsService(bookingRepo.Object);

            var service = new CapacityService(_accommodationRepository, bookings);

            var result = await service.FindAvailableAccommodation(DateTime.Now.AddDays(4), DateTime.Now.AddDays(10), 3);

            // три комнаты
            Assert.AreEqual(3, result.Count);

            // три цены
            Assert.AreEqual(3, result.SelectMany(p => p.Prices).Count());

            // максимально на одного человека
            Assert.AreEqual(1, result.SelectMany(p => p.Prices).Max(p => p.Accommodation.Capacity));
        }

        [Test]
        public async Task Assure_Services_Returns_Two_Rooms_With_Double_And_Singles()
        {
            var bookingRepo = new Mock<IBookingRepository>();
            // 3 комната занята всегда, в 1 живет 1 человек, во второй 1 сменяется на другого и есть запрос на овербукинг
            bookingRepo.Setup(p => p.GetBookingsByConditionAsync(It.IsAny<Func<Order, bool>>())).ReturnsAsync(
                new List<Order>()
                {
                    CreateTypicalOrder(DateTime.Now.AddDays(5), DateTime.Now.AddDays(30), 1, 3, OrderStatus.Confirmed),
                    CreateTypicalOrder(DateTime.Now.AddDays(5), DateTime.Now.AddDays(7), 1, 2, OrderStatus.Confirmed),
                    CreateTypicalOrder(DateTime.Now.AddDays(8), DateTime.Now.AddDays(20), 1, 2, OrderStatus.Confirmed),
                    CreateTypicalOrder(DateTime.Now.AddDays(5), DateTime.Now.AddDays(20), 2, 2, OrderStatus.Request, 3),
                    CreateTypicalOrder(DateTime.Now.AddDays(6), DateTime.Now.AddDays(20), 1, 1, OrderStatus.Confirmed),
                }
            );

            var bookings = new BookingsService(bookingRepo.Object);

            var service = new CapacityService(_accommodationRepository, bookings);

            var result = await service.FindAvailableAccommodation(DateTime.Now.AddDays(4), DateTime.Now.AddDays(10), 2);

            // комнаты 2
            Assert.AreEqual(2, result.Count);

            // только 3 цены (на 2х + 1 и на 1)
            Assert.AreEqual(3, result.SelectMany(p => p.Prices).Count());

            // вместимость до 2 человек
            Assert.AreEqual(2, result.SelectMany(p => p.Prices).Max(p => p.Accommodation.Capacity));
        }
    } 
}
