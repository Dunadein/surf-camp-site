using Microsoft.AspNetCore.Mvc;
using SurfLevel.Contracts.Interfaces.Services;
using SurfLevel.Contracts.ViewModels;
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

        [HttpGet("list")]
        public async Task<IEnumerable<ViewPackage>> GetPackages()
        {
            return await _packageProvider.GetPackageListAsync();
        }
    }
}
