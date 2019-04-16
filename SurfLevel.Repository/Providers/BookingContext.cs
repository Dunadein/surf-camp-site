using Microsoft.EntityFrameworkCore;
using SurfLevel.Contracts.Models.DatabaseObjects;

namespace SurfLevel.Repository.Providers
{
    public class BookingContext : DbContext
    {
        public BookingContext(DbContextOptions<BookingContext> options) 
            : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }

        public DbSet<Guest> Tourists { get; set; }
    }
}
