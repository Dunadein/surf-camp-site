using SurfLevel.Contracts.Interfaces.Services;

namespace SurfLevel.Domain.Providers
{
    public class LocalizedSpaStaticFilePathProvider
    {
        private readonly ILocaleService _userLanguageService;
        private readonly string _distFolder;

        public LocalizedSpaStaticFilePathProvider(ILocaleService userLanguageService, string distFolder)
        {
            _userLanguageService = userLanguageService;
            _distFolder = distFolder;
        }

        public string GetRequestPath(string subpath)
        {
            var userLocale = _userLanguageService.GetUserLocale();
            var spaFilePath = (string.IsNullOrEmpty(_distFolder) ? "" : "/" + _distFolder) + "/" + userLocale + subpath;
            return spaFilePath;
        }
    }
}
