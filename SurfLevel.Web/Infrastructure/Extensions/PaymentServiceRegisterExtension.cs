using Microsoft.Extensions.DependencyInjection;
using SurfLevel.Contracts.Interfaces.Services;
using SurfLevel.Domain.Options;
using SurfLevel.Domain.Payments;
using System;
using YandexPaymentProvider;
using YandexPaymentProvider.Interfaces;

namespace SurfLevel.Web.Infrastructure.Extensions
{
    internal static class PaymentServiceRegisterExtension
    {
        public static void RegisterPaymentService(this IServiceCollection services, string connectionString, PaymentOptions options)
        {
            services.AddHttpClient<IPaymentService, YandexPaymentService>(client =>
            {
                client.BaseAddress = new Uri(options.BankApiURI);
                client.Timeout = TimeSpan.FromSeconds(5);
            });

            services.AddHttpClient<IYandexPaymentProvider, PaymentProvider>(client =>
            {
                client.BaseAddress = new Uri(options.ProviderApiURI);
                client.Timeout = TimeSpan.FromSeconds(5);
            });

            services.AddYandexProvider(connectionString);
        }
    }
}
