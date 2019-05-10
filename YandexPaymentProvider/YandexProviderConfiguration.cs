using Microsoft.Extensions.DependencyInjection;
using YandexPaymentProvider.Interfaces;
using YandexPaymentProvider.Repository;

namespace YandexPaymentProvider
{
    public static class YandexProviderConfiguration
    {
        public static IServiceCollection AddYandexProvider(this IServiceCollection services, string connectionString)
        {            
            services.AddSingleton<IYandexProviderRepository>(repo => 
                new YandexProviderRepository(connectionString));

            return services;
        }
    }
}
