using SurfLevel.Domain.ViewModels.Package;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SurfLevel.Domain.IProviders
{
    public interface IPackageProvider
    {
        Task<List<ViewPackage>> GetPackageListAsync();
    }
}
