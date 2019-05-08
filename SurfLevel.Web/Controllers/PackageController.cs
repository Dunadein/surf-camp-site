using Microsoft.AspNetCore.Mvc;
using SurfLevel.Domain.Providers.Interfaces;
using SurfLevel.Domain.ViewModels.Package;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SurfLevel.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PackageController : ControllerBase
    {
        private readonly IPackageProvider _packageProvider;

        public PackageController(IPackageProvider packageProvider)
        {
            _packageProvider = packageProvider;
        }

        [HttpGet("get-packages")]
        public async Task<IEnumerable<ViewPackage>> GetPackages()
            => await _packageProvider.GetPackageListAsync();
    }
}
