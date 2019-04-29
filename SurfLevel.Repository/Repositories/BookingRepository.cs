using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using SurfLevel.Contracts.Interfaces.Repositories;
using SurfLevel.Contracts.Models.DatabaseObjects;
using SurfLevel.Repository.DBProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurfLevel.Repository.Repositories
{
    internal static partial class Extension
    {
        public static IIncludableQueryable<Order, Accommodation> EndChain(this IQueryable<Order> chain)
        {
            return chain.Include(p => p.Guests).Include(p => p.Services).ThenInclude(p => p.Package)
                .Include(p => p.Services).ThenInclude(p => p.AccommodationPrice).ThenInclude(p => p.Accommodation);               
        }
    }

    public class BookingRepository : IBookingRepository
    {
        private readonly BookingContext _context;
        private readonly IAccommodationRepository _accomodation;
        private readonly IPackageRepository _package;

        public BookingRepository(BookingContext context, IAccommodationRepository accomodationRepository, IPackageRepository packageRepository)
        {
            _context = context;
            _accomodation = accomodationRepository;
            _package = packageRepository;
        }

        public async Task<List<Order>> GetBookingsAsync(DateTime? startFrom = null)
        {
            return await _context.Orders.Where(p =>
                startFrom.HasValue ? p.DateFrom >= startFrom.Value : true
            ).EndChain().AsNoTracking().ToListAsync();
        }

        public async Task<List<Order>> GetBookingsInPeriodAsync(DateTime periodStart, DateTime? periodEnd = null)
        {
            return await _context.Orders.Where(p =>
                p.DateTill >= periodStart && (periodEnd.HasValue ? p.DateFrom <= periodEnd.Value : true)
            ).EndChain().AsNoTracking().ToListAsync();
        }        

        public async Task<int> CreateBookingAsync(Order order)
        {
            _context.Orders.Add(order);

            await _context.SaveChangesAsync();

            return order.Id;
        }
    }
}
