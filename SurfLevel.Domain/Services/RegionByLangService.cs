using Microsoft.AspNetCore.Http;
using SurfLevel.Contracts.Interfaces.Services;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace SurfLevel.Domain.Services
{
    public class RegionByLangService : IRegionByLangService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly List<string> _availableLanguages;
        private readonly string _localeCookie;

        public RegionByLangService(IHttpContextAccessor httpContextAccessor
            , IEnumerable<string> availableLanguages
            , string localeCookieName)
        {
            _httpContextAccessor = httpContextAccessor;
            _availableLanguages = availableLanguages.Select(l => l.ToLowerInvariant()).ToList();
            _localeCookie = localeCookieName;
        }

        public string GetUserLocale()
        {
            var request = _httpContextAccessor.HttpContext?.Request;
            if (request == null)
            {
                return _availableLanguages[0];
            }

            var cookieLocale = request.Cookies[_localeCookie];
            if (!string.IsNullOrWhiteSpace(cookieLocale))
            {
                return cookieLocale;
            }

            var userLocales = request.Headers["Accept-Language"].ToString();
            var userAcceptLanguage = GetAcceptLanguageFromHeaderOrNull(userLocales);

            if (!string.IsNullOrWhiteSpace(userAcceptLanguage))
            {
                return userAcceptLanguage;
            }

            return _availableLanguages[0];
        }

        public string GetAcceptLanguageFromHeaderOrNull(string headerValue)
        {
            if (headerValue == null)
            {
                return null;
            }
            try
            {
                var clientLanguages = (headerValue).Split(',')
                    .Select(StringWithQualityHeaderValue.Parse)
                    .OrderByDescending(language => language.Quality.GetValueOrDefault(1))
                    .Select(language => language.Value.ToLowerInvariant())
                    .Select(languageCode => languageCode.Contains("-") 
                        ? languageCode.Split('-').First() : languageCode)
                    .Distinct()
                    .Where(languageCode => !string.IsNullOrWhiteSpace(languageCode) && languageCode.Trim() != "*");

                return clientLanguages
                    .FirstOrDefault(clientLanguage => _availableLanguages.Contains(clientLanguage));
            }
            catch
            {
                return null;
            }
        }
    }
}
