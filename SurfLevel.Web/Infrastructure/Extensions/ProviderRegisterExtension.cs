using Microsoft.Extensions.DependencyInjection;
using SurfLevel.Domain.Providers;
using SurfLevel.Domain.Providers.Interfaces;

namespace SurfLevel.Web.Infrastructure.Extensions
{
    internal static class ProviderRegisterExtension
    {
        public static void RegisterProviders(this IServiceCollection services)
        {
            services.AddTransient<IBookingProvider, BookingProvider>();
            services.AddTransient<ICalendarProvider, CalendarProvider>();
            services.AddTransient<IOrderProvider, OrderProvider>();
            services.AddTransient<IPackageProvider, PackageProvider>();
            services.AddTransient<IPaymentCallback, PaymentCallback>();
            services.AddTransient<ISearchProvider, SearchProvider>();
        }
    }
}
