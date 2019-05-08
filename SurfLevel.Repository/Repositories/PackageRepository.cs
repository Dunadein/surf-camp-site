using Microsoft.EntityFrameworkCore;
using SurfLevel.Contracts.Interfaces.Repositories;
using SurfLevel.Contracts.Models.DatabaseObjects;
using SurfLevel.Repository.DBProviders;
using System;
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

        public async Task<List<Package>> GetPackagesByConditionAsync(Func<Package, bool> condition = null)
        {
            return await _context.Packages.Include(p => p.OutOfServicePeriods).Include(p => p.PackagePrices)
                .AsNoTracking().Where(p => condition != null ? condition(p) : true).ToListAsync();
        }

        public async Task<Package> GetPackageByConditionAsync(Func<Package, bool> condition)
        {
            return await _context.Packages.Include(p => p.OutOfServicePeriods).Include(p => p.PackagePrices)
                .AsNoTracking().FirstOrDefaultAsync(p => condition(p));
        }
    }
}
