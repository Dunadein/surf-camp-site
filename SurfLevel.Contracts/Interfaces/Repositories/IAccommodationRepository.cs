using SurfLevel.Contracts.Models.DatabaseObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SurfLevel.Contracts.Interfaces.Repositories
{
    public interface IAccommodationRepository
    {
        Task<List<Villa>> GetAccommodationsAsync(Func<Villa, bool> condition = null);

        Task<Room> GetRoomByIdAsync(int roomId);

        Task<AccommodationPrice> GetPriceByCondition(Func<AccommodationPrice, bool> condition);
    }
}
