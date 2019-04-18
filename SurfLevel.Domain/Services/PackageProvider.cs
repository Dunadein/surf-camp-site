using SurfLevel.Contracts.Interfaces.Repositories;
using SurfLevel.Contracts.Interfaces.Services;
using SurfLevel.Contracts.Models.ViewModels.Packages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurfLevel.Domain.Services
{
    public class PackageProvider : IPackageProvider
    {
        private readonly IPackageRepository _repository;
        private readonly IRegionByLangService _langService;

        public PackageProvider(IPackageRepository packageRepository, IRegionByLangService langService)
        {
            _repository = packageRepository;
            _langService = langService;
        }

        public async Task<IEnumerable<ViewPackage>> GetPackageListAsync()
        {
            var packages = await _repository.GetAllPackagesAsync(false);

            var currentLocale = _langService.GetUserLocale();

            return new List<ViewPackage>(packages.Select(p => new ViewPackage()
            {
                Label = p.ShortLabel,
                LocaleFolder = currentLocale,
                Price = p.IsWithAccommodation ? p.PromoPrice ?? p.MinDayPrice * 7 : p.MinDayPrice,
                IsDefault = p.IsWithAccommodation && p.IsDefault
            }));
        }
    }
}
