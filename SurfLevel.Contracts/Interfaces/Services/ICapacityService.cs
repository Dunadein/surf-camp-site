using SurfLevel.Contracts.Models.DatabaseObjects;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SurfLevel.Contracts.Interfaces.Services
{
    public interface ICapacityService
    {
        Task<List<Room>> FindAvailableAccommodation(DateTime periodStart, DateTime periodEnd, int pax);
    }
}
