using SurfLevel.Domain.ViewModels.Calendar;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SurfLevel.Domain.Providers.Interfaces
{
    public interface ICalendarProvider
    {
        Task<List<CalendarDate>> GetCalendarDates();
    }
}
