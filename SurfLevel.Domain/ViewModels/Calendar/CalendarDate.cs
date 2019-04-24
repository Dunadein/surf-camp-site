using System;

namespace SurfLevel.Domain.ViewModels.Calendar
{
    public class CalendarDate
    {
        public DateTime Date { get; set; }
        public LoadingState LoadingState { get; set; }
    }

    public enum LoadingState
    {
        BookedOut,
        Few,
        Ok,
        Many
    }
}
