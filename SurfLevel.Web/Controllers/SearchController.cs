using Microsoft.AspNetCore.Mvc;
using SurfLevel.Contracts.Interfaces.Services;
using SurfLevel.Contracts.Models.ViewModels.Search;
using System.Threading.Tasks;

namespace SurfLevel.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private readonly ISearchProvider _searchProvider;

        public SearchController(ISearchProvider searchProvider)
        {
            _searchProvider = searchProvider;
        }

        [HttpGet("get-hash")]
        public async Task<string> GetHashedRequest([FromBody]SearchRequest request)
            => await _searchProvider.GetHashedRequest(request);

        [HttpGet("get-packages/{hash:string?}")]
        public async Task<SearchPackagesResult> GetAvailablePackages([FromRoute]string hash)
            => await _searchProvider.SearchAvailablePackages(hash);
    }
}