using SurfLevel.Contracts.Interfaces.Repositories;
using SurfLevel.Contracts.Interfaces.Services;
using SurfLevel.Contracts.ViewModels.Search;
using System;
using System.Threading.Tasks;

namespace SurfLevel.Domain.Services
{
    public class SearchProvider : ISearchProvider
    {
        private readonly IPackageRepository _packageRepository;

        public SearchProvider(IPackageRepository packageRepository)
        {
            _packageRepository = packageRepository;
        }

        public async Task<SearchResult> SearchAvailableResources(SearchRequest request)
        {
            request.Validate();

            var packages = await _packageRepository.GetAllPackagesAsync(false);

            return await Task.FromResult<SearchResult>(null);
        }
    }
}
