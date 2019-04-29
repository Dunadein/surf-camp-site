using SurfLevel.Contracts.Models.DatabaseObjects;
using SurfLevel.Contracts.Models.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SurfLevel.Contracts.Interfaces.Services
{
    public interface IPricingService
    {
        Task<List<PaxPrice>> GetServicePricesAsync(Package package);

        Task<Dictionary<int, List<PaxPrice>>> GetAccommodationPricesAsync(Package package, DateTime from, DateTime till, int pax);

        Task<decimal> CalculatePackageMinPriceAsync(Package package, DateTime? from = null, DateTime? till = null);

        Task<Tuple<int?, decimal>> CalculateRequestedPriceAsync(Package package, int? roomId, int servicePax, DateTime from, DateTime? till = null);
    }
}
