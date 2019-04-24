using SurfLevel.Domain.ViewModels.Calendar;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SurfLevel.Contracts.Interfaces.Providers
{
    public interface ICalendarProvider
    {
        Task<List<CalendarDate>> GetCalendarDates();
    }
}
