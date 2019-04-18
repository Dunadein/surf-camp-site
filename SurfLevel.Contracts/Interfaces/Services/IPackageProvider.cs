using SurfLevel.Contracts.Models.ViewModels.Packages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SurfLevel.Contracts.Interfaces.Services
{
    public interface IPackageProvider
    {
        Task<IEnumerable<ViewPackage>> GetPackageListAsync();
    }
}
