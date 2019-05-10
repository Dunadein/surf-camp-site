using Moq;
using NUnit.Framework;
using SurfLevel.Contracts.Interfaces.Repositories;
using SurfLevel.Contracts.Interfaces.Services;
using SurfLevel.Contracts.Models.DatabaseObjects;
using SurfLevel.Domain.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurfLevel.Domain.Test
{
    public class PricingServiceTest
    {
        private const decimal DAY_PRICE = 30m;

        private IPricingService _pricingService;

        [SetUp]
        public void SetUp()
        {
            var accommodation = new Mock<IAccommodationRepository>();

            accommodation.Setup(p => p.GetPriceByConditionAsync(
                It.IsAny<Func<AccommodationPrice, bool>>(), 
                It.IsAny<Func<IQueryable<AccommodationPrice>, IOrderedQueryable<AccommodationPrice>>>()
            )).ReturnsAsync(new AccommodationPrice()
            {
                DayPrice = DAY_PRICE
            });

            accommodation.Setup(p => p.GetRoomByConditionAsync(
                It.IsAny<Func<Room, bool>>()
            )).ReturnsAsync(GetRoom());

            var capacity = new Mock<ICapacityService>();

            capacity.Setup(p => p.FindAvailableAccommodation(
                It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<int>()
            )).ReturnsAsync(new List<Room>() { GetRoom() });

            _pricingService = new PricingService(capacity.Object, accommodation.Object);
        }

        [Test]
        public async Task Assure_Service_Returns_Accommodation_Promo_Price()
        {
            var package = new Package()
            {
                IsWithAccommodation = true,
                MinDayPrice = DAY_PRICE,
                PackagePrices = new List<PackagePeriodPrice>()
                {
                    new PackagePeriodPrice()
                    {
                        PeriodFrom = 1,
                        PeriodTill = 10,
                        Price = -20
                    }
                }
            };

            var result = await _pricingService.CalculatePackageMinPriceAsync(package);

            Assert.AreEqual(400, result);
        }

        [Test]
        public async Task Assure_Service_Returns_Service_Day_Price()
        {
            var package = new Package()
            {
                MinDayPrice = 35
            };

            var result = await _pricingService.CalculatePackageMinPriceAsync(package);

            Assert.AreEqual(35, result);
        }

        [Test]
        public async Task Assure_Service_Returns_Accommodation_Calculated_Price()
        {
            var package = new Package()
            {
                IsWithAccommodation = true,
                MinDayPrice = DAY_PRICE,
                PackagePrices = new List<PackagePeriodPrice>()
                {
                    new PackagePeriodPrice()
                    {
                        PeriodFrom = 7,
                        PeriodTill = 7,
                        Price = -20
                    }
                }
            };

            var result = await _pricingService.CalculateRequestedPriceAsync(package, 1, 2, DateTime.Now, DateTime.Now.AddDays(9));

            Assert.AreEqual(1080, result.Item2);
        }

        [Test]
        public async Task Assure_Service_Returns_Service_Calculated_Price()
        {
            var package = new Package()
            {                
                MinDayPrice = 35,
                PackagePrices = new List<PackagePeriodPrice>()
                {
                    new PackagePeriodPrice()
                    {
                        PeriodFrom = 1,
                        PeriodTill = 3,
                        Price = 45
                    },
                    new PackagePeriodPrice()
                    {
                        PeriodFrom = 4,
                        Price = 40
                    }
                }
            };

            var result = await _pricingService.CalculateRequestedPriceAsync(package, 1, 4, DateTime.Now);

            Assert.AreEqual(160, result.Item2);
        }

        [Test]
        public async Task Assure_Service_Returns_Service_Price_Period()
        {
            var package = new Package()
            {
                MinDayPrice = 35,
                PackagePrices = new List<PackagePeriodPrice>()
                {
                    new PackagePeriodPrice()
                    {
                        PeriodFrom = 1,
                        PeriodTill = 2,
                        Price = 50
                    },
                    new PackagePeriodPrice()
                    {
                        PeriodFrom = 4,
                        PeriodTill = 4,
                        Price = 45
                    },
                    new PackagePeriodPrice()
                    {
                        PeriodFrom = 5,
                        Price = 40
                    }
                }
            };

            var result = await _pricingService.GetServicePricesAsync(package);
            
            // цены представлены на весь период из 9 дней
            Assert.AreEqual(9, result.Count);

            // пропуск в ценах заполнен ближайшей дорогой ценой
            Assert.AreEqual(150, result.FirstOrDefault(p => p.Pax == 3).Price);
        }

        [Test]
        public async Task Assure_Service_Returns_Accommodation_With_Prices()
        {
            var package = new Package()
            {
                IsWithAccommodation = true,
                MinDayPrice = DAY_PRICE,
                PackagePrices = new List<PackagePeriodPrice>()
                {
                    new PackagePeriodPrice()
                    {
                        PeriodFrom = 7,
                        PeriodTill = 7,
                        Price = 10
                    }
                }
            };

            var result = await _pricingService.GetAccommodationPricesAsync(package, DateTime.Now, DateTime.Now.AddDays(7), 2);

            Assert.AreEqual(2, result.SelectMany(p => p.Value).Count());

            Assert.AreEqual(430, result[0].FirstOrDefault(p => p.Pax == 1).Price);

            Assert.AreEqual(860, result[0].FirstOrDefault(p => p.Pax == 2).Price);
        }

        private Room GetRoom()
        {
            return new Room()
            {
                Prices = new List<AccommodationPrice>()
                {
                    new AccommodationPrice()
                    {
                        DayPrice = 30,
                        Accommodation = new Accommodation()
                        {
                            Capacity = 1,
                        }
                    },
                    new AccommodationPrice()
                    {
                        DayPrice = 60,
                        Accommodation = new Accommodation()
                        {
                            Capacity = 2,
                        }
                    }
                }
            };
        }
    }
}
