using SurfLevel.Contracts.Models.DatabaseObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SurfLevel.Domain.Test.Common
{
    public class AccommodationSetupWithOrderStub : AccommodationSetup
    {
        protected Order CreateTypicalOrder(DateTime from, DateTime? till, int pax, int? roomId, OrderStatus orderStatus, int? accommodationType = null)
        {
            var randId = new Random().Next(1, int.MaxValue - 10);
            return new Order()
            {
                Id = randId,
                DateFrom = from.Date,
                DateTill = till?.Date,
                GuestsCount = pax,
                Status = orderStatus,
                Guests = Enumerable.Range(1, pax).Select(p => new Guest()).ToArray(),
                Services = new List<Service>
                {
                    new Service()
                    {
                        OrderId = randId,
                        ServiceDays = till.HasValue ? (till.Value - from).Days + 1 : 5,
                        PackageId = 1,
                        AccommodationPriceId = roomId.HasValue ? GetAccommodationPrice(roomId.Value, accommodationType ?? pax).Id : (int?)null,
                        AccommodationPrice = roomId.HasValue ? GetAccommodationPrice(roomId.Value, accommodationType ?? pax) : null
                    }
                }
            };
        }

        private AccommodationPrice GetAccommodationPrice(int roomId, int pax)
        {
            return GetAccommodationPrices().FirstOrDefault(p => p.RoomId == roomId && p.Accommodation.Сapacity == pax);
        }
    }
}
