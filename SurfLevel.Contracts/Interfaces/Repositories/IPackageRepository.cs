using SurfLevel.Contracts.Models.DatabaseObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SurfLevel.Contracts.Interfaces.Repositories
{
    public interface IPackageRepository
    {
        Task<IEnumerable<Package>> GetAllPackagesAsync();

        Task<Package> GetPackageByIdAsync(int id);
    }
}
