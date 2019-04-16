using Microsoft.AspNetCore.Mvc;
using SurfLevel.Contracts.Interfaces.Services;
using SurfLevel.Contracts.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SurfLevel.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalendarController : ControllerBase
    {
        private readonly ICalendarProvider _provider;

        public CalendarController(ICalendarProvider calendarProvider)
        {
            _provider = calendarProvider;
        }

        [HttpGet("get-dates")]
        public async Task<IEnumerable<CalendarDate>> GetDatesForCalendar()
            => await _provider.GetCalendarDates();
    }
}