using System;

namespace SurfLevel.Contracts.Models.ViewModels.Search
{
    public class SearchRequest : Request
    {
        public override void Validate()
        {
            if (!WithAccommodation && !Till.HasValue)
                throw new ArgumentException("End date wasn't provide");

            if (DateTime.UtcNow.Date >= From.Date)
                throw new ArgumentException($"The From Date of value {From.ToShortDateString()} couldn't be served die to booking policy");
        }
    }
}
