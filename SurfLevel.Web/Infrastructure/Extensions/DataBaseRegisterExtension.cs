using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SurfLevel.Contracts.Interfaces.Repositories;
using SurfLevel.Repository.DBProviders;
using SurfLevel.Repository.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurfLevel.Web.Infrastructure.Extensions
{
    internal static class DataBaseRegisterExtension
    {
        public static void RegisterRepositories(this IServiceCollection services, string connectionString)
        {
            services.AddEntityFrameworkMySql()
                .AddDbContext<BookingContext>(options => options.UseMySql(connectionString))
                .AddDbContext<AccommodationContext>(options => options.UseMySql(connectionString))
                .AddDbContext<PackageContext>(options => options.UseMySql(connectionString))
                .AddDbContext<PaymentContext>(options => options.UseMySql(connectionString));

            services.AddTransient<IAccommodationRepository, AccommodationRepository>();
            services.AddTransient<IBookingRepository, BookingRepository>();
            services.AddTransient<IPackageRepository, PackageRepository>();
            services.AddTransient<IPaymentRepository, PaymentRepository>();
        }
    }
}
