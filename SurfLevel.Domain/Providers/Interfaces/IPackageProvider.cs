using SurfLevel.Domain.ViewModels.Package;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SurfLevel.Domain.Providers.Interfaces
{
    public interface IPackageProvider
    {
        Task<List<ViewPackage>> GetPackageListAsync();
    }
}
