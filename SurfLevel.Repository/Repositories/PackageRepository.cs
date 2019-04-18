using Microsoft.EntityFrameworkCore;
using SurfLevel.Contracts.Interfaces.Repositories;
using SurfLevel.Contracts.Models.DatabaseObjects;
using SurfLevel.Contracts.Models.ViewModels;
using SurfLevel.Repository.Providers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurfLevel.Repository.Repositories
{
    public class PackageRepository : IPackageRepository
    {
        private readonly PackageContext _context;

        public PackageRepository(PackageContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Package>> GetAllPackagesAsync(bool onlyWithAccommodation = true)
        {
            return await _context.Packages.AsNoTracking()
                .Where(p => onlyWithAccommodation ? p.IsWithAccommodation : true)
                .ToListAsync();
        }

        public async Task<IEnumerable<Package>> GetAvailableForPeriodPackagesAsync(Request request)
        {
            return await _context.Packages.Include(p => p.OutOfServicePeriods).AsNoTracking()
                .Where(p => !p.OutOfServicePeriods.Any(t => t.Start <= request.From && t.End >= request.Till))
                .ToListAsync();
        }

        public async Task<Package> GetPackageByIdOrDefaultAsync(Request request, int? id = null)
        {
            return await _context.Packages.Include(p => p.OutOfServicePeriods).AsNoTracking()
                .FirstOrDefaultAsync(p => 
                (
                    id.HasValue ? p.Id == id.Value
                    : p.IsWithAccommodation == request.WithAccommodation && p.IsDefault
                ) && !p.OutOfServicePeriods.Any(t => t.Start <= request.From && t.End >= request.Till)
            );
        }
    }
}
