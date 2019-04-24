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

        decimal CalculatePackagePromoPrice(Package package, DateTime from, DateTime? till = null);

        decimal CalculatePackageWithAccommodationPrice(Package package, Room r);
    }
}
