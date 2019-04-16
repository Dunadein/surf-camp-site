using System;

namespace SurfLevel.Contracts.Models.ViewModels
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
