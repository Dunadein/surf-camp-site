using Microsoft.EntityFrameworkCore;
using SurfLevel.Contracts.Models.DatabaseObjects;

namespace SurfLevel.Repository.DBProviders
{
    public class AccommodationContext : DbContext
    {
        public AccommodationContext(DbContextOptions<AccommodationContext> options)
            : base(options)
        {
        }

        public DbSet<Villa> Villas { get; set; }

        public DbSet<Room> Rooms { get; set; }

        public DbSet<Accommodation> Accommodations { get; set; }

        public DbSet<AccommodationPrice> AccommodationPrices { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccommodationPrice>(entity =>
            {
                entity.ToTable("AccommodationPrice");
                entity.HasOne(p => p.Accommodation).WithMany()
                    .HasForeignKey(r => r.AccommodationId);                
            });

            modelBuilder.Entity<Villa>(entity =>
            {
                entity.ToTable("Villa");
                entity.HasMany(p => p.Rooms).WithOne()
                    .HasForeignKey(p => p.VillaId);
            });

            modelBuilder.Entity<Room>(entity =>
            {
                entity.ToTable("Room");
                entity.HasMany(p => p.Prices).WithOne()
                    .HasForeignKey(p => p.RoomId);
            });

            modelBuilder.Entity<Accommodation>(entity =>
            {
                entity.ToTable("Accommodation");
            });
        }
    }
}
