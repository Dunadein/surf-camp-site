using SurfLevel.Contracts.Models.DatabaseObjects;
using SurfLevel.Contracts.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SurfLevel.Contracts.Interfaces.Repositories
{
    public interface IPackageRepository
    {
        Task<IEnumerable<Package>> GetAllPackagesAsync(bool onlyWithAccommodation = true);

        Task<IEnumerable<Package>> GetAvailableForPeriodPackagesAsync(Request request);

        Task<Package> GetPackageByIdOrDefaultAsync(Request request, int? id = null);
    }
}
