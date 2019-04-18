using SurfLevel.Contracts.Interfaces.Repositories;
using SurfLevel.Contracts.Interfaces.Services;
using SurfLevel.Contracts.Models.ViewModels.Packages;
using SurfLevel.Contracts.Models.ViewModels.Search;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SurfLevel.Domain.Services
{
    public class SearchProvider : ISearchProvider
    {
        private readonly IPackageRepository _packageRepository;
        private readonly ISearchHasherService _hasher;
        private readonly IRegionByLangService _locale;

        public SearchProvider(IPackageRepository packageRepository,
            ISearchHasherService hasherService, IRegionByLangService localeService)
        {
            _packageRepository = packageRepository;
            _hasher = hasherService;
            _locale = localeService;
        }

        private SearchRequest GetRequestFromHash(string hash)
        {
            // дефолтный реквест
            if (string.IsNullOrEmpty(hash))
            {
                return new SearchRequest()
                {
                    From = DateTime.UtcNow.AddDays(1).Date,
                    Till = DateTime.UtcNow.AddDays(8).Date,
                    Pax = 2,
                    WithAccommodation = true
                };
            }

            var request = _hasher.Read<SearchRequest>(hash);
            request.Validate();

            return request;
        }

        public async Task<SearchPackagesResult> SearchAvailablePackages(string hash)
        {
            var request = GetRequestFromHash(hash);

            var locale = _locale.GetUserLocale();

            var packages = await _packageRepository.GetAvailableForPeriodPackagesAsync(request);

            var result = new SearchPackagesResult()
            {
                Packages = packages.Select(p => new ViewPackageWithId()
                {
                    Id = p.Id,
                    Label = p.ShortLabel,
                    LocaleFolder = locale,
                    WithAccommodation = p.IsWithAccommodation,
                    IsDefault = p.IsDefault
                }).ToList()
            };

            return result;
        }

        public async Task<string> GetHashedRequest(SearchRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("Request data wasn't provided");

            request.Validate();

            return await Task.Run(() => _hasher.Create(request));
        }

        public async Task<SearchPriceByPackageResult> SearchPricesByPackageId(string hash, int? packageId = null)
        {
            var request = GetRequestFromHash(hash);

            var package = await _packageRepository.GetPackageByIdOrDefaultAsync(request, packageId);

            if (package == null)
                throw new ArgumentNullException("Package not found");

            // если не надо искать цены на жилье
            if (!package.IsWithAccommodation)
            {
                return new SearchPriceByPackageResult()
                {
                    Prices = 
                };
            }
        }
    }
}
