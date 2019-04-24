using Microsoft.EntityFrameworkCore;
using SurfLevel.Contracts.Interfaces.Repositories;
using SurfLevel.Contracts.Models.DatabaseObjects;
using SurfLevel.Repository.DBProviders;
using System.Collections.Generic;
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

        public async Task<IEnumerable<Villa>> GetAccommodationsAsync()
        {
            return await _context.Villas.Include(p => p.Rooms)
                .ThenInclude(p => p.Prices).ThenInclude(p => p.Accommodation).ToListAsync();
        }

        public async Task<Room> GetRoomByIdAsync(int roomId)
        {
            return await _context.Rooms.Include(p => p.Prices).ThenInclude(p => p.Accommodation)
                .FirstOrDefaultAsync(p => p.Id == roomId);
        }
    }
}
