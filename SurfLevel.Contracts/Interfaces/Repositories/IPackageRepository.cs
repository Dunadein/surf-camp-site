using SurfLevel.Contracts.Models.DatabaseObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SurfLevel.Contracts.Interfaces.Repositories
{
    public interface IPackageRepository
    {
        Task<List<Package>> GetPackagesByConditionAsync(Func<Package, bool> condition = null);

        Task<Package> GetPackageByConditionAsync(Func<Package, bool> condition);
    }
}
