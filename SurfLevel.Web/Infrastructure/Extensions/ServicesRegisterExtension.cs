using Microsoft.Extensions.DependencyInjection;
using SurfLevel.Contracts.Interfaces.Services;
using SurfLevel.Domain.Providers.Interfaces;
using SurfLevel.Domain.Services;
using System.Linq;

namespace SurfLevel.Web.Infrastructure.Extensions
{
    internal static class ServicesRegisterExtension
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.Scan(scan => scan.FromAssemblyOf<IHashedProvider>().AddClasses(filter =>
                filter.AssignableTo(typeof(IHasherService<>))).AsImplementedInterfaces()
                    .WithScopedLifetime());

            services.AddScoped<IBookingsService, BookingsService>();
            services.AddScoped<ICapacityService, CapacityService>();
            services.AddScoped<IPricingService, PricingService>();
        }
    }
}
