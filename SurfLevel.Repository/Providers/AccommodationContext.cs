using Microsoft.EntityFrameworkCore;
using SurfLevel.Contracts.Models.DatabaseObjects;

namespace SurfLevel.Repository.Providers
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
            modelBuilder.Entity<AccommodationPrice>().HasKey(x => new { x.RoomId, x.AccommodationId });
        }
    }
}
