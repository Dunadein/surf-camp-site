using SurfLevel.Contracts.Models.DatabaseObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SurfLevel.Contracts.Interfaces.Repositories
{
    public interface IAccommodationRepository
    {
        Task<List<Villa>> GetAccommodationsAsync(Func<Villa, bool> condition = null);

        Task<Room> GetRoomByConditionAsync(Func<Room, bool> condition);

        Task<AccommodationPrice> GetPriceByConditionAsync(Func<AccommodationPrice, bool> condition, Func<IQueryable<AccommodationPrice>, IOrderedQueryable<AccommodationPrice>> sorter = null);        
    }
}
