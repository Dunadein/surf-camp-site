using SurfLevel.Contracts.Interfaces.Repositories;
using SurfLevel.Contracts.Interfaces.Services;
using SurfLevel.Contracts.Models.DatabaseObjects;
using SurfLevel.Contracts.Models.DTO;
using SurfLevel.Domain.IProviders;
using SurfLevel.Domain.ViewModels.Package;
using SurfLevel.Domain.ViewModels.Search;
using SurfLevel.Domain.ViewModels.Search.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurfLevel.Domain
{
    public class SearchProvider : ISearchProvider
    {
        private readonly IPackageRepository _packageRepository;
        private readonly IAccommodationRepository _accommodationRepository;
        private readonly ISearchHasherService _hasher;
        private readonly IRegionByLangService _locale;
        private readonly IPricingService _pricing;

        public SearchProvider(IPackageRepository packageRepository,
            IAccommodationRepository accommodationRepository,
            ISearchHasherService hasherService,
            IRegionByLangService localeService,
            IPricingService pricingService)
        {
            _packageRepository = packageRepository;
            _accommodationRepository = accommodationRepository;
            _hasher = hasherService;
            _locale = localeService;
            _pricing = pricingService;
        }

        private SearchRequest GetRequestFromHashOrDefault(string hash)
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

        private async Task<IEnumerable<Package>> GetPackagesByRequest(SearchRequest request)
        {
            var packages = await _packageRepository.GetAllPackagesAsync();

            return packages.Where(p => p.IsWithAccommodation == request.WithAccommodation
                && !p.OutOfServicePeriods.Any(t => t.Start <= request.From && t.End >= request.Till));
        }

        private async Task<Package> GetPackageById(SearchRequest request, int packageId)
        {
            var package = await _packageRepository.GetPackageByIdAsync(packageId);

            if (package == null)
                throw new ArgumentNullException("The package not found.");

            if (!package.OutOfServicePeriods.Any(t => t.Start <= request.From && t.End >= request.Till))
                throw new ArgumentException("The package is not available.");

            return package;
        }

        public async Task<SearchRequest> GetRequestFromHashAsync(string hash = null)
        {
            return await Task.Run(() => GetRequestFromHashOrDefault(hash));
        }

        public async Task<string> GetHashedRequestAsync(SearchRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("Request data wasn't provided");

            request.Validate();

            return await Task.Run(() => _hasher.Create(request));
        }

        public async Task<List<ViewPackageWithId>> SearchAvailablePackagesAsync(string hash)
        {
            var request = GetRequestFromHashOrDefault(hash);

            var locale = _locale.GetUserLocale();

            var packages = await GetPackagesByRequest(request);

            return packages.Select(p => new ViewPackageWithId()
            {
                Id = p.Id,
                Label = p.ShortLabel,
                LocaleFolder = locale,
                WithAccommodation = p.IsWithAccommodation,
                IsDefault = p.IsDefault
            }).ToList();
        }

        public async Task<List<PaxPrice>> SearchPackagePrices(string hash, int packageId)
        {
            if (string.IsNullOrWhiteSpace(hash))
                throw new ArgumentNullException("Request data wasn't provided");

            var request = GetRequestFromHashOrDefault(hash);

            var package = await GetPackageById(request, packageId);

            return await _pricing.GetServicePricesAsync(package);
        }

        public async Task<Dictionary<int, List<PaxPrice>>> SearchPackageWithAccommodationPrices(string hash, int packageId)
        {
            if(string.IsNullOrWhiteSpace(hash))
                throw new ArgumentNullException("Request data wasn't provided");

            var request = GetRequestFromHashOrDefault(hash);

            var package = await GetPackageById(request, packageId);

            return await _pricing.GetAccommodationPricesAsync(package, request.From, request.Till.Value, request.Pax);
        }

        public async Task<SearchByDefaultResult> SearchByDefaultAsync(string hash)
        {
            var request = GetRequestFromHashOrDefault(hash);

            var packageList = await GetPackagesByRequest(request);

            var package = packageList.FirstOrDefault(p => p.IsDefault) ??
                packageList.OrderByDescending(p => p.MinDayPrice).FirstOrDefault();

            if (package == null)
                throw new ArgumentNullException("Failed to find any package.");

            // если не надо искать цены на жилье
            if (!request.WithAccommodation)
            {
                return new SearchByDefaultResult() { Prices = await _pricing.GetServicePricesAsync(package) };
            }

            var result = new SearchByDefaultResult()
            {
                Villas = new List<ViewVilla>()
            };

            var suitable = await _pricing.GetAccommodationPricesAsync(package, request.From, request.Till.Value, request.Pax);

            if (suitable.Count == 0)
                return result;

            var accommodations = await _accommodationRepository.GetAccommodationsAsync();

            var locale = _locale.GetUserLocale();

            foreach (var villa in accommodations.Where(p => p.IsEnabled))
            {
                if (villa.Rooms.Any(p => suitable.ContainsKey(p.Id)))
                {
                    var viewVilla = new ViewVilla()
                    {
                        FolderName = villa.FolderName,
                        IsDefault = villa.IsDefault,
                        Name = villa.Name,
                        Locale = locale
                    };

                    foreach(var room in villa.Rooms.Where(p => suitable.ContainsKey(p.Id)))
                    {
                        viewVilla.Rooms.Add(new ViewRoom()
                        {
                            Folder = room.DescriptionFolder,
                            Id = room.Id,
                            Prices = suitable[room.Id],
                            MaxPax = room.Prices.Max(p => p.Accommodation.Сapacity)
                        });
                    }

                    result.Villas.Add(viewVilla);
                }
            }

            return result;
        }
    }
}
