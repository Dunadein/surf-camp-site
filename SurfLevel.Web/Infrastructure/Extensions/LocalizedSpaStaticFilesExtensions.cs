using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SurfLevel.Contracts.Interfaces.Services;
using SurfLevel.Domain.Services;

namespace SurfLevel.Web.Infrastructure.Extensions
{
    public static class LocalizedSpaStaticFilesExtensions
    {
        public static void AddLocalizedSpaStaticFiles(this IServiceCollection services,
            string[] availableLocales, string spaRootPath, string localeCookieName)
        {
            services.AddTransient<IRegionByLangService>(serviceProvider =>
            {
                var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
                return new RegionByLangService(httpContextAccessor, availableLocales, localeCookieName);
            });
            services.AddTransient(serviceProvider =>
            {
                var userLanguageService = serviceProvider.GetRequiredService<IRegionByLangService>();
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
