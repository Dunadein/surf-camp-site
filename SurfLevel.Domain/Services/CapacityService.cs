using SurfLevel.Contracts.Interfaces.Repositories;
using SurfLevel.Contracts.Interfaces.Services;
using SurfLevel.Contracts.Models.DatabaseObjects;
using SurfLevel.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurfLevel.Domain.Services
{
    public class CapacityService : ICapacityService
    {
        private readonly IAccommodationRepository _accommodations;
        private readonly IBookingsService _bookings;

        public CapacityService(IAccommodationRepository accommodationRepository, IBookingsService bookingsService)
        {
            _accommodations = accommodationRepository;
            _bookings = bookingsService;
        }

        private async Task<List<AccommodationPrice>> GetAvailableVariants(IEnumerable<Room> rooms, DateTime periodStart, DateTime periodEnd)
        {
            var bookings = await _bookings.GetBookingsWithDates(periodStart, periodEnd, false);

            var occupied = bookings.Where(p => p.Date.IsBetween(periodStart, periodEnd)).SelectMany(p => p.Groupping)
                .GroupBy(p => p.RoomKey).Where(p => p.Key.HasValue).Select(p => new
                {
                    RoomId = p.Key,
                    Taken = p.Max(group => group.Pax)
                });
            
            // доступный номерной фонд
            var prices = rooms.SelectMany(p => p.Prices.Select(t => new
                {
                    Price = t,
                    MaxPax = p.Prices.Max(m => m.Accommodation.Capacity)
                }))
                .Where(p => !occupied.Any(t => t.RoomId == p.Price.RoomId && p.MaxPax - t.Taken < p.Price.Accommodation.Capacity))
                .Select(p => p.Price).ToList();

            return prices;
        }

        public async Task<List<Room>> FindAvailableAccommodation(DateTime periodStart, DateTime periodEnd, int pax)
        {
            var accommodations = await _accommodations.GetAccommodationsAsync(p => p.IsEnabled);

            var rooms = accommodations.SelectMany(p => p.Rooms).ToList();

            var prices = await GetAvailableVariants(rooms, periodStart, periodEnd);

            var notIncludedRooms = new List<int>();
            // если можно поселить всю группу на виллу без ограничений по съему
            if (accommodations.Any(p => !p.MinPaxForRent.HasValue
                && p.Rooms.Any(t => prices.Any(s => s.RoomId == t.Id && s.Accommodation.Capacity >= pax))))
            {
                notIncludedRooms = accommodations.Where(p => p.MinPaxForRent.HasValue)
                    .SelectMany(v => v.Rooms.Select(t => t.Id)).ToList();
            }

            var result = new List<Room>();
            foreach (var groupedPrices in prices.GroupBy(p => p.RoomId))
            {
                if (!notIncludedRooms.Contains(groupedPrices.Key))
                {
                    var baseRoom = rooms.FirstOrDefault(p => p.Id == groupedPrices.Key);

                    var room = new Room()
                    {
                        DescriptionFolder = baseRoom.DescriptionFolder,
                        Id = baseRoom.Id,
                        Name = baseRoom.Name,
                        VillaId = baseRoom.VillaId
                    };

                    foreach(var price in groupedPrices)
                    {
                        // если эта комната позволяет поселить всю группу, то добавляются только цены на всю группу
                        // или добавляются цены, которые позволяют в паре с другими комнатами расселить всех
                        if (price.Accommodation.Capacity < pax && (
                            groupedPrices.Any(p => p.Accommodation.Capacity >= pax)
                            && price.Accommodation.Capacity + prices.GroupBy(p => p.RoomId)
                                .Sum(t => t.Key == price.RoomId ? 0 : t.Max(s => s.Accommodation.Capacity)) < pax
                        ))
                            continue;                        
                        
                        room.Prices.Add(price);
                    }

                    result.Add(room);
                }
            }

            return result;
        }
    }
}
