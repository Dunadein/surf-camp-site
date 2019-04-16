using Microsoft.EntityFrameworkCore;
using SurfLevel.Contracts.Interfaces.Repositories;
using SurfLevel.Contracts.Models.DatabaseObjects;
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
    }
}
