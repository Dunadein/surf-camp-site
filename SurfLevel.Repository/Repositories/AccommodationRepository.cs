using Microsoft.EntityFrameworkCore;
using SurfLevel.Contracts.Interfaces.Repositories;
using SurfLevel.Contracts.Models.DatabaseObjects;
using SurfLevel.Repository.Providers;
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

        public async Task<IEnumerable<Villa>> GetAccommodationsAsync()
        {
            var villas = await _context.Villas.Include(p => p.Rooms).ToListAsync();

            var accommodationType = await _context.Accommodations.AsNoTracking().ToListAsync();

            foreach (var villa in villas)
            {
                foreach(var room in villa.Rooms)
                {
                    _context.AccommodationPrices.AsNoTracking().Where(p => p.RoomId == room.Id).Load();
                    //room.Accommodations.Add()
                }
            }

            return villas;
        }
    }
}
