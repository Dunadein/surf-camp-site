using SurfLevel.Contracts.Interfaces.Repositories;
using SurfLevel.Contracts.Interfaces.Services;
using SurfLevel.Domain.IProviders;
using SurfLevel.Domain.ViewModels.Package;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurfLevel.Domain
{
    public class PackageProvider : IPackageProvider
    {       
        private readonly IPackageRepository _repository;
        private readonly IRegionByLangService _langService;
        private readonly IPricingService _pricing;

        public PackageProvider(IPackageRepository packageRepository, 
            IRegionByLangService langService,
            IPricingService pricingService)
        {
            _repository = packageRepository;
            _langService = langService;
            _pricing = pricingService;
        }

        public async Task<List<ViewPackage>> GetPackageListAsync()
        {
            var packages = await _repository.GetAllPackagesAsync();

            var currentLocale = _langService.GetUserLocale();

            return new List<ViewPackage>(packages.Select(p => new ViewPackage()
            {
                Label = p.ShortLabel,
                LocaleFolder = currentLocale,
                Price = _pricing.CalculatePackagePromoPrice(p, DateTime.Now),
                IsDefault = p.IsWithAccommodation && p.IsDefault
            }));
        }
    }
}
