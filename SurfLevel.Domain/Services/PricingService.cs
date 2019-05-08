using SurfLevel.Contracts.Interfaces.Repositories;
using SurfLevel.Contracts.Interfaces.Services;
using SurfLevel.Contracts.Models.DatabaseObjects;
using SurfLevel.Contracts.Models.DTO;
using SurfLevel.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static SurfLevel.Domain.Fetching.PrimaryKeyStrategy;

namespace SurfLevel.Domain.Services
{
    public class PricingService : IPricingService
    {
        private const int DefaultMaxPeriod = 28;
        private const int PromoDays = 7;

        private readonly ICapacityService _capacity;
        private readonly IAccommodationRepository _accommodation;

        public PricingService(ICapacityService capacityService,
            IAccommodationRepository accommodationRepository)
        {           
            _capacity = capacityService;
            _accommodation = accommodationRepository;
        }

        public async Task<decimal> CalculatePackageMinPriceAsync(Package package, DateTime? from = null, DateTime? till = null)
        {
            if (package == null)
                throw new ArgumentNullException("Package not found");

            if (!package.IsWithAccommodation)
                return package.MinDayPrice;

            return await Task.Run(() => CalculatePrice(package, GetDuration(from, till)));
        }

        public async Task<Tuple<int?, decimal>> CalculateRequestedPriceAsync(Package package, int? roomId, int servicePax, DateTime from, DateTime? till = null)
        {
            if (package == null)
                throw new ArgumentNullException("Package not found");

            if (!package.IsWithAccommodation)
                return new Tuple<int?, decimal>(null, CalculatePrice(package, servicePax));

            if (!roomId.HasValue)
                throw new ArgumentException($"The package {package.Name} is with Accommodation, but it wasn't supplied.");

            var room = await _accommodation.GetRoomByConditionAsync(GetById<Room>(roomId.Value));

            var roomPrice = room?.Prices.FirstOrDefault(p => p.Accommodation.Сapacity == servicePax);

            if (roomPrice == null)
                throw new ArgumentNullException("Can't find requested room price.");

            return new Tuple<int?, decimal>(roomPrice.Id, CalculatePrice(package, GetDuration(from, till), roomPrice.DayPrice));
        }

        public async Task<List<PaxPrice>> GetServicePricesAsync(Package package)
        {
            if (package == null)
                throw new ArgumentNullException("Package not found");

            if (package.IsWithAccommodation)
                throw new ArgumentException($"The package {package.Name} is with Accommodation");

            if (package.PackagePrices.Count == 0)
                throw new ArgumentException($"There is no actual price for the package {package.Name}");

            // строим период цен
            var range = Enumerable.Range(Math.Min(package.PackagePrices.Min(p => p.PeriodStart), 1), package.PackagePrices.Max(p => p.PeriodEnd ?? DefaultMaxPeriod));
            
            return await Task.Run(() => range.Select(p => new PaxPrice()
            {
                Pax = p,
                Price = CalculatePrice(package, p)
            }).ToList());
        }

        public async Task<Dictionary<int, List<PaxPrice>>> GetAccommodationPricesAsync(Package package, DateTime from, DateTime till, int pax)
        {
            if (package == null)
                throw new ArgumentNullException("Package not found");

            if (!package.IsWithAccommodation)
                throw new ArgumentException($"The package {package.Name} is without Accommodation");

            var suitable = await _capacity.FindAvailableAccommodation(from, till, pax);

            var duration = GetDuration(from, till);

            return suitable.ToDictionary(
                p => p.Id,
                p => p.Prices.Select(t => new PaxPrice()
                {
                    Pax = t.Accommodation.Сapacity,
                    Price = CalculatePrice(package, duration, t.DayPrice)
                }).ToList()
            );
        }

        private decimal CalculatePrice(Package package, int duration, decimal? accommodationPrice = null)
        {
            var price = package.PackagePrices.OrderBy(p => p.PeriodStart)
                .FirstOrDefault(p => duration.IsBetween(p.PeriodStart, p.PeriodEnd ?? DefaultMaxPeriod))?.Price;

            if (price.HasValue)
                return price.Value + (accommodationPrice ?? 0) * duration;

            // если в периоде была дыра, будем давать ближайшие снизу (с более короткого периода) цены
            if (!package.IsWithAccommodation)
            {
                var lastTry = package.PackagePrices.OrderByDescending(p => p.PeriodStart)
                    .FirstOrDefault(p => (p.PeriodEnd ?? DefaultMaxPeriod) < duration)?.Price;

                if (lastTry.HasValue)
                    return lastTry.Value;
            }

            return (package.MinDayPrice + (accommodationPrice ?? 0)) * duration;
        }

        private int GetDuration(DateTime? from = null, DateTime? till = null)
        {
            return till.HasValue && from.HasValue ? (till.Value - from.Value).Days : PromoDays;
        }
    }
}
