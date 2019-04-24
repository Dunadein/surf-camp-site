using Microsoft.EntityFrameworkCore;
using SurfLevel.Contracts.Interfaces.Repositories;
using SurfLevel.Contracts.Models.DatabaseObjects;
using SurfLevel.Repository.DBProviders;
using System.Collections.Generic;
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

        public async Task<IEnumerable<Package>> GetAllPackagesAsync()
        {
            return await _context.Packages.Include(p => p.OutOfServicePeriods).Include(p => p.PackagePrices)
                .AsNoTracking().ToListAsync();
        }

        public async Task<Package> GetPackageByIdAsync(int id)
        {
            return await _context.Packages.Include(p => p.OutOfServicePeriods).Include(p => p.PackagePrices)
                .AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
