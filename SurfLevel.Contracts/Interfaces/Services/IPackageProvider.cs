using SurfLevel.Contracts.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SurfLevel.Contracts.Interfaces.Services
{
    public interface IPackageProvider
    {
        Task<IEnumerable<ViewPackage>> GetPackageListAsync();
    }
}
