using Microsoft.AspNetCore.Mvc;
using SurfLevel.Contracts.Interfaces.Services;
using SurfLevel.Contracts.ViewModels.Search;
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

        [HttpGet("")]
        public async Task<SearchResult> GetAvailablePackages([FromBody]SearchRequest request)
            => await _searchProvider.SearchAvailableResources(request);
    }
}