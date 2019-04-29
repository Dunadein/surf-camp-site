using Moq;
using NUnit.Framework;
using SurfLevel.Contracts.Interfaces.Repositories;
using SurfLevel.Contracts.Models.DatabaseObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SurfLevel.Domain.Test.Common
{
    public class AccommodationSetup
    {
        protected IAccommodationRepository _accommodationRepository;

        [SetUp]
        public virtual void Setup()
        {
            var accommodationRepository = new Mock<IAccommodationRepository>();

            #region Long-Long-Level-Setup
            accommodationRepository.Setup(p => p.GetAccommodationsAsync(It.IsAny<Func<Villa, bool>>())).ReturnsAsync(
                new List<Villa>()
                {
                    new Villa()
                    {
                        IsEnabled = true,
                        Rooms = new List<Room>()
                        {
                            new Room()
                            {
                                Id = 1,                                
                                Prices = GetAccommodationPrices().Where(p => p.RoomId == 1).ToList()
                            },
                            new Room()
                            {
                                Id = 2,
                                Prices = GetAccommodationPrices().Where(p => p.RoomId == 2).ToList()
                            },
                            new Room()
                            {
                                Id = 3,
                                Prices = GetAccommodationPrices().Where(p => p.RoomId == 3).ToList()
                            }
                        }
                    }
                }
            );
            #endregion

            _accommodationRepository = accommodationRepository.Object;
        }

        protected Accommodation GetAccommodation(int pax)
        {
            return new Accommodation()
            {
                Id = pax,
                Сapacity = pax
            };
        }

        protected List<AccommodationPrice> GetAccommodationPrices()
        {
            return new List<AccommodationPrice>()
            {
                new AccommodationPrice() { RoomId = 1, AccommodationId = 1, Id = 1, Accommodation = GetAccommodation(1) },
                new AccommodationPrice() { RoomId = 1, AccommodationId = 2, Id = 2, Accommodation = GetAccommodation(2) },
                new AccommodationPrice() { RoomId = 2, AccommodationId = 1, Id = 3, Accommodation = GetAccommodation(1) },
                new AccommodationPrice() { RoomId = 2, AccommodationId = 2, Id = 4, Accommodation = GetAccommodation(2) },
                new AccommodationPrice() { RoomId = 2, AccommodationId = 3, Id = 5, Accommodation = GetAccommodation(3) },
                new AccommodationPrice() { RoomId = 3, AccommodationId = 1, Id = 6, Accommodation = GetAccommodation(1) }
            };
        }
    }
}
