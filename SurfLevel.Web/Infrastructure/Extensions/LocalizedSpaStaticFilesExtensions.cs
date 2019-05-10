using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SurfLevel.Contracts.Interfaces.Services;
using SurfLevel.Domain.Options;
using SurfLevel.Domain.Providers;
using SurfLevel.Domain.Services;

namespace SurfLevel.Web.Infrastructure.Extensions
{
    internal static class LocalizedSpaStaticFilesExtensions
    {
        public static void AddLocalizedSpaStaticFiles(this IServiceCollection services,
            LocalizationSettings settings,
            string spaRootPath)
        {
            services.AddTransient<ILocaleService>(serviceProvider =>
            {
                var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
                return new LocaleByLangService(
                    httpContextAccessor,
                    settings.SupportedLocales,
                    settings.LocaleCookieName
                );
            });
            services.AddTransient(serviceProvider =>
            {
                var userLanguageService = serviceProvider.GetRequiredService<ILocaleService>();
                return new LocalizedSpaStaticFilePathProvider(userLanguageService, spaRootPath);
            });
        }

        public static void UseLocalizedSpaStaticFiles(this IApplicationBuilder applicationBuilder, string defaultFile)
        {
            applicationBuilder.Use((context, next) =>
            {                
                var spaFilePathProvider = context.RequestServices.GetRequiredService<LocalizedSpaStaticFilePathProvider>();
                context.Request.Path = spaFilePathProvider.GetRequestPath("/" + defaultFile.TrimStart('/'));

                return next();
            });

            applicationBuilder.UseStaticFiles();
        }
    }
}
