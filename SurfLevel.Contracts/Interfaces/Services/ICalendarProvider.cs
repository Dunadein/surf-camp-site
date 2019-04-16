using SurfLevel.Contracts.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SurfLevel.Contracts.Interfaces.Services
{
    public interface ICalendarProvider
    {
        Task<IEnumerable<CalendarDate>> GetCalendarDates();
    }
}
