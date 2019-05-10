using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SurfLevel.Domain.Options;
using YandexPaymentProvider.Options;

namespace SurfLevel.Web.Infrastructure.Extensions
{
    internal static class OptionsConfigurationExtension
    {
        public static void ConfigurateOptions(this IServiceCollection services, IConfiguration config)
        {
            services.AddOptions();
            services.Configure<CommissionLocales>(config);
            services.Configure<PaymentOptions>(config);
            services.Configure<YandexPaymentOptions>(config);
            services.Configure<LocalizationSettings>(config);
        }
    }
}
