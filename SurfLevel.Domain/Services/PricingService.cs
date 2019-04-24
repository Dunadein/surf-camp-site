using SurfLevel.Contracts.Interfaces.Services;
using SurfLevel.Contracts.Models.DatabaseObjects;
using SurfLevel.Contracts.Models.DTO;
using SurfLevel.Domain.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurfLevel.Domain.Services
{
    public class PricingService : IPricingService
    {
        private const int DefaultMaxPeriod = 28;
        private const int PromoDays = 7;

        private readonly ICapacityService _capacity;

        public PricingService(ICapacityService capacityService)
        {           
            _capacity = capacityService;
        }

        public decimal CalculatePackagePromoPrice(Package package, DateTime from, DateTime? till = null)
        {
            if (package == null)
                throw new ArgumentNullException("Package not found");

            if (!package.IsWithAccommodation)
                return package.MinDayPrice;
           
            return CalculatePrice(package, from, till);
        }

        public decimal CalculatePackageWithAccommodationPrice(Package package, Room room, DateTime from, DateTime? till = null)
        {
            if (package == null)
                throw new ArgumentNullException("Package not found");

            if (package.IsWithAccommodation && room == null)
                throw new ArgumentException($"The package {package.Name} is with Accommodation, but is wasn't supplied.");

            return CalculatePrice(package, from, till);
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
            var range = Enumerable.Range(package.PackagePrices.Min(p => p.PeriodStart), package.PackagePrices.Max(p => p.PeriodEnd ?? DefaultMaxPeriod));
            // если в периоде была дыра, будем давать ближайшие сверху (с более короткого периода) цены
            return await Task.Run(() => range.Select(p => new PaxPrice()
            {
                Pax = p,
                Price = package.PackagePrices.OrderBy(pp => pp.PeriodStart)
                    .FirstOrDefault(pp => p.IsBetween(pp.PeriodStart, pp.PeriodEnd ?? DefaultMaxPeriod))?.Price
                        ?? package.PackagePrices.OrderBy(pp => pp.PeriodStart)
                            .FirstOrDefault(pp => (pp.PeriodEnd ?? DefaultMaxPeriod) < p).Price
            }).ToList());
        }

        public async Task<Dictionary<int, List<PaxPrice>>> GetAccommodationPricesAsync(Package package, DateTime from, DateTime till, int pax)
        {
            if (package == null)
                throw new ArgumentNullException("Package not found");

            if (!package.IsWithAccommodation)
                throw new ArgumentException($"The package {package.Name} is without Accommodation");

            var suitable = await _capacity.FindAvailableAccommodation(from, till, pax);

            return suitable.ToDictionary(p => p.Id,
                p => p.Prices.Select(t => new PaxPrice()
                {
                    Pax = t.Accommodation.Сapacity,
                    Price = CalculatePrice(package, from, till, t.DayPrice)
                }).ToList()
            );
        }

        private decimal CalculatePrice(Package package, DateTime from, DateTime? till, decimal? accommodationPrice = null)
        {
            var duration = till.HasValue ? (till.Value - from).Days : PromoDays;

            var price = package.PackagePrices.FirstOrDefault(p => duration.IsBetween(p.PeriodStart, p.PeriodEnd ?? DefaultMaxPeriod));

            if (price != null)
                return price.Price;

            return (package.MinDayPrice + (accommodationPrice ?? 0)) * duration;
        }
    }
}
