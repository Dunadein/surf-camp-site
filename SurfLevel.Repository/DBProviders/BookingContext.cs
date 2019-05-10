using Microsoft.EntityFrameworkCore;
using SurfLevel.Contracts.Models.DatabaseObjects;

namespace SurfLevel.Repository.DBProviders
{
    public class BookingContext : DbContext
    {
        public BookingContext(DbContextOptions<BookingContext> options)
            : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Guest> Tourists { get; set; }

        public DbSet<Service> Services { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>(entity =>
            {
                entity.ToTable("Order");
                entity.HasMany(p => p.Guests).WithOne()
                    .HasForeignKey(t => t.OrderId);
                entity.HasMany(p => p.Services).WithOne()
                    .HasForeignKey(t => t.OrderId);
            });

            modelBuilder.Entity<Guest>(entity =>
            {
                entity.ToTable("Guest");
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.ToTable("Service");
                entity.HasOne(p => p.Package).WithMany()
                    .HasForeignKey(t => t.PackageId);
                entity.HasOne(p => p.AccommodationPrice).WithMany()
                    .HasForeignKey(t => t.AccommodationPriceId);
            });
        }
    }
}
