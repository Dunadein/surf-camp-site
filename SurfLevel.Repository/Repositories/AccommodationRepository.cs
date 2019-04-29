using Microsoft.EntityFrameworkCore;
using SurfLevel.Contracts.Interfaces.Repositories;
using SurfLevel.Contracts.Models.DatabaseObjects;
using SurfLevel.Repository.DBProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurfLevel.Repository.Repositories
{
    public class AccommodationRepository : IAccommodationRepository
    {
        private readonly AccommodationContext _context;

        public AccommodationRepository(AccommodationContext context)
        {
            _context = context;
        }

        public async Task<List<Villa>> GetAccommodationsAsync(Func<Villa, bool> condition = null)
        {
            return await _context.Villas.Include(p => p.Rooms)
                .ThenInclude(p => p.Prices).ThenInclude(p => p.Accommodation)
                .Where(p => condition != null ? condition(p) : true).ToListAsync();
        }

        public async Task<Room> GetRoomByIdAsync(int roomId)
        {
            return await _context.Rooms.Include(p => p.Prices).ThenInclude(p => p.Accommodation)
                .FirstOrDefaultAsync(p => p.Id == roomId);
        }

        public async Task<AccommodationPrice> GetPriceByCondition(Func<AccommodationPrice, bool> condition)
        {
            return await _context.AccommodationPrices.Include(p => p.Accommodation)
                .FirstOrDefaultAsync(p => condition(p));
        }
    }
}
