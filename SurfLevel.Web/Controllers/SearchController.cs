using Microsoft.AspNetCore.Mvc;
using SurfLevel.Contracts.Models.DTO;
using SurfLevel.Domain.Providers.Interfaces;
using SurfLevel.Domain.ViewModels.Package;
using SurfLevel.Domain.ViewModels.Search;
using System.Collections.Generic;
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

        [HttpPost("create-hash")]
        public async Task<string> MakeHashedRequest([FromBody]SearchRequest request)
            => await _searchProvider.GetHashedRequestAsync(request);

        [HttpGet("get-request/{hash?}")]
        public async Task<SearchRequest> GetSearchRequest([FromRoute]string hash)
            => await _searchProvider.GetRequestFromHashAsync(hash);

        [HttpGet("get-available-packages/{hash?}")]
        public async Task<IEnumerable<ViewPackageWithId>> GetAvailablePackages([FromRoute]string hash)
            => await _searchProvider.SearchAvailablePackagesAsync(hash);

        [HttpGet("get-service-prices/{hash}")]
        public async Task<IEnumerable<PaxPrice>> GetServicePackagePrices([FromRoute]string hash, [FromBody]int packageId)
            => await _searchProvider.SearchPackagePrices(hash, packageId);

        [HttpGet("get-accommodation-prices/{hash}")]
        public async Task<IDictionary<int, List<PaxPrice>>> GetServiceWithAccommodationPackagePrices([FromRoute]string hash, [FromBody]int packageId)
            => await _searchProvider.SearchPackageWithAccommodationPrices(hash, packageId);

        [HttpGet("{hash?}")]
        public async Task<SearchByDefaultResult> GetDefaultSearchResult([FromRoute]string hash)
            => await _searchProvider.SearchByDefaultAsync(hash);
    }
}