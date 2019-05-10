using Microsoft.EntityFrameworkCore;
using SurfLevel.Contracts.Models.DatabaseObjects;

namespace SurfLevel.Repository.DBProviders
{
    public class PackageContext : DbContext
    {
        public PackageContext(DbContextOptions<PackageContext> options)
            : base(options)
        {
        }

        public DbSet<Package> Packages { get; set; }

        public DbSet<PackagePeriodPrice> PackagePrices { get; set; }

        public DbSet<OutOfServicePeriod> StopPeriods { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Package>(entity =>
            {
                entity.ToTable("Package");
                entity.HasMany(p => p.PackagePrices).WithOne()
                    .HasForeignKey(t => t.PackageId);
                entity.HasMany(p => p.OutOfServicePeriods).WithOne()
                    .HasForeignKey(t => t.PackageId);
            });

            modelBuilder.Entity<PackagePeriodPrice>(entity =>
            {
                entity.ToTable("PackagePrices");
            });

            modelBuilder.Entity<OutOfServicePeriod>(entity =>
            {
                entity.ToTable("StopPeriod");
            });
        }
    }
}
