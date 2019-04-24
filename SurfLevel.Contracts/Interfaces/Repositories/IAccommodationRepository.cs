using SurfLevel.Contracts.Models.DatabaseObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SurfLevel.Contracts.Interfaces.Repositories
{
    public interface IAccommodationRepository
    {
        Task<IEnumerable<Villa>> GetAccommodationsAsync();

        Task<Room> GetRoomByIdAsync(int roomId);
    }
}
