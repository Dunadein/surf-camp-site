using Microsoft.EntityFrameworkCore;
using SurfLevel.Contracts.Interfaces.Repositories;
using SurfLevel.Contracts.Models.DatabaseObjects;
using SurfLevel.Repository.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurfLevel.Repository.Repositories
{
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

        public async Task<IEnumerable<Order>> GetBookingsAsync(DateTime? startFrom = null)
        {
            return await _context.Orders.Where(p => startFrom.HasValue ? p.DateFrom >= startFrom.Value : true)
                .Include(p => p.Guests).AsNoTracking().ToListAsync();
        }
    }
}
