using Moq;
using NUnit.Framework;
using SurfLevel.Contracts.Interfaces.Repositories;
using SurfLevel.Contracts.Models.DatabaseObjects;
using SurfLevel.Contracts.Models.ViewModels;
using SurfLevel.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurfLevel.Domain.Test
{
    public class CalendarProviderTest
    {
        private IAccommodationRepository _accommodationRepository;

        [SetUp]
        public void Setup()
        {
            var accommodationRepository = new Mock<IAccommodationRepository>();

            #region Long-Long-Level-Setup
            accommodationRepository.Setup(p => p.GetAccommodationsAsync()).ReturnsAsync(
                new List<Villa>()
                {
                    new Villa()
                    {
                        IsEnabled = true,
                        Rooms = new List<Room>()
                        {
                            new Room()
                            {
                                Accommodations = new List<Accommodation>() { new Accommodation() { �apacity = 2 }  }
                            },
                            new Room()
                            {
                                Accommodations = new List<Accommodation>()
                                {
                                    new Accommodation() { �apacity = 2 },
                                    new Accommodation() { �apacity = 3 }
                                }
                            },
                            new Room()
                            {
                                Accommodations = new List<Accommodation>() { new Accommodation() { �apacity = 1 }  }
                            }
                        }
                    }
                }
            );
            #endregion

            _accommodationRepository = accommodationRepository.Object;
        }

        [Test]
        public async Task Assure_All_Orders_Calculate_Correctly()
        {
            var bookingRepository = new Mock<IBookingRepository>();

            // ������� �� ������� �������� � 6 ���� � ���� �� ���� (����� ������ �� �����)
            bookingRepository.Setup(p => p.GetBookingsAsync(It.IsAny<DateTime?>())).ReturnsAsync(
                new List<Order>()
                {
                    CreateTypicalOrder(DateTime.Now.AddDays(5), DateTime.Now.AddDays(12), 2, OrderStatus.Confirmed),
                    CreateTypicalOrder(DateTime.Now.AddDays(5), DateTime.Now.AddDays(7), 1, OrderStatus.Confirmed),
                    CreateTypicalOrder(DateTime.Now.AddDays(7), DateTime.Now.AddDays(14), 2, OrderStatus.Confirmed, 3),
                    CreateTypicalOrder(DateTime.Now.AddDays(7), DateTime.Now.AddDays(14), 2, OrderStatus.Annulated, 3),
                    CreateTypicalOrder(DateTime.Now.AddDays(15), DateTime.Now.AddDays(20), 1, OrderStatus.Payed, 2),
                    CreateTypicalOrder(DateTime.Now.AddDays(20), DateTime.Now.AddDays(24), 1, OrderStatus.Request, 3),
                    CreateTypicalOrder(DateTime.Now.AddDays(20), null, 2, OrderStatus.Request), // �� ������ ����������� � ��������,
                    CreateTypicalOrder(DateTime.Now.AddDays(30), null, 2, OrderStatus.Request) // ������ ����������� � ��������
                }
            );

            var provider = new CalendarProvider(bookingRepository.Object, _accommodationRepository);

            var result = await provider.GetCalendarDates();

            Assert.NotNull(result);
            // ���� ������ ��� ����
            Assert.IsTrue(result.Any(p => p.LoadingState == LoadingState.BookedOut));
            // � �� ����� ����, ��������, ��� ��� ���������� ��� ����� �� ��� ����.
            Assert.LessOrEqual(result.Count(p => p.LoadingState == LoadingState.BookedOut), 1);
            // ���� ������ ����� ���� (��� ���� ����� ������)
            Assert.IsTrue(result.Any(p => p.LoadingState == LoadingState.Ok));
            // ���� ����� ��������� ���� �� ��������� ����
            Assert.Greater(result.Count(p => p.LoadingState == LoadingState.Many), 300);
            // ����������� ����� ��� ���������� (����� ��� � �� �� ���� ������ � �����������)
            Assert.IsTrue(result.Where(p => p.Date.CompareTo(DateTime.Now.AddDays(30)) >= 0).Any(p => p.LoadingState == LoadingState.Ok));
        }

        private Order CreateTypicalOrder(DateTime from, DateTime? till, int pax, OrderStatus orderStatus, int? accommodationType = null)
        {
            var randId = new Random().Next(1, int.MaxValue - 10);
            return new Order()
            {
                Id = randId,
                DateFrom = from.Date,
                DateTill = till?.Date,
                GuestsCount = pax,
                Status = orderStatus,
                Guests = new List<Guest>
                (
                    Enumerable.Range(1, pax).Select(p => new Guest()
                    {
                        OrderId = randId,
                        Duration = till.HasValue ? (till.Value - from).Days + 1 : 5,
                        PackageId = 1,
                        AccommodationPriceId = till.HasValue ? accommodationType ?? pax : (int?)null,
                        AccommodationPrice = till.HasValue ? new AccommodationPrice()
                        {
                            Id = accommodationType ?? pax,
                            Accommodation = GetAccommodation(accommodationType ?? pax),
                            AccommodationId = accommodationType ?? pax
                        } : null
                    })
                )
            };
        }

        private Accommodation GetAccommodation(int pax)
        {
            return new Accommodation()
            {
                Id = pax,
                �apacity = pax
            };
        }
    }
}