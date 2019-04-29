using SurfLevel.Contracts.Interfaces.Repositories;
using SurfLevel.Contracts.Interfaces.Services;
using SurfLevel.Contracts.Models.DatabaseObjects;
using SurfLevel.Domain.Providers.Interfaces;
using SurfLevel.Domain.ViewModels.Package;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurfLevel.Domain.Providers
{
    public class PackageProvider : IPackageProvider
    {       
        private readonly IPackageRepository _repository;
        private readonly ILocaleService _langService;
        private readonly IPricingService _pricing;

        public PackageProvider(IPackageRepository packageRepository, 
            ILocaleService langService,
            IPricingService pricingService)
        {
            _repository = packageRepository;
            _langService = langService;
            _pricing = pricingService;
        }

        public async Task<List<ViewPackage>> GetPackageListAsync()
        {
            var packages = await _repository.GetPackagesAsync();

            var currentLocale = _langService.GetUserLocale();

            return new List<ViewPackage>(await Task.WhenAll(packages.Select(p => ConvertFromPackage(p, currentLocale))));
        }

        private async Task<ViewPackage> ConvertFromPackage(Package pac, string locale)
        {
            var price = await _pricing.CalculatePackageMinPriceAsync(pac);

            return new ViewPackage()
            {
                Label = pac.ShortLabel,
                LocaleFolder = locale,
                Price = price,
                IsDefault = pac.IsWithAccommodation && pac.IsDefault
            };
        }
    }
}
