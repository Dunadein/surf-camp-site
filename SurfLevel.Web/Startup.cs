using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SurfLevel.Repository.DBProviders;
using SurfLevel.Web.Infrastructure.Extensions;

namespace SurfLevel.Web
{
    public class Startup
    {
        public Startup(IHostingEnvironment environment)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(environment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddHttpContextAccessor();
            services.AddLocalizedSpaStaticFiles(
                availableLocales: Configuration.GetSection("SupportedLocales").Get<string[]>(),
                spaRootPath: "dist",
                localeCookieName: Configuration["LocaleCookieName"]
            );

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });

            services.AddEntityFrameworkMySql()
            .AddDbContext<BookingContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("DBConnection"))
            )
            .AddDbContext<AccommodationContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("DBConnection"))
            )
            .AddDbContext<PackageContext>(options =>
                options.UseMySql(Configuration.GetConnectionString("DBConnection"))
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSpaStaticFiles();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();            

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });            

            if (env.IsDevelopment())
            {
                app.UseSpa(spa =>
                { 
                    spa.Options.SourcePath = "ClientApp";
                    spa.UseAngularCliServer(npmScript: "start");
                });
            }
            else
            {
                app.UseLocalizedSpaStaticFiles("index.html");
            }
        }
    }
}
