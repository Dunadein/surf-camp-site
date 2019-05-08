using SurfLevel.Contracts.Models.DTO;
using SurfLevel.Domain.ViewModels.Package;
using SurfLevel.Domain.ViewModels.Search;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SurfLevel.Domain.Providers.Interfaces
{
    public interface ISearchProvider
    {
        Task<string> GetHashedRequestAsync(SearchRequest request);

        Task<SearchRequest> GetRequestFromHashAsync(string hash = null);

        Task<List<ViewPackageWithId>> SearchAvailablePackagesAsync(string hash);

        Task<SearchByDefaultResult> SearchByDefaultAsync(string hash);

        Task<List<PaxPrice>> SearchPackagePrices(string hash, int packageId);

        Task<Dictionary<int, List<PaxPrice>>> SearchPackageWithAccommodationPrices(string hash, int packageId);
    }
}
