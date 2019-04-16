using SurfLevel.Contracts.Models.ViewModels;
using System;

namespace SurfLevel.Contracts.ViewModels.Search
{
    public class SearchRequest : Request
    {
        public DateTime From { get; set; }
        public DateTime? Till { get; set; }
        public int Pax { get; set; }
        public bool JustLessons { get; set; }

        public override void Validate()
        {
            if (!JustLessons && !Till.HasValue)
                throw new ArgumentException("End date wasn't provide");

            if (DateTime.UtcNow > From)
                throw new ArgumentException($"The From Date of value {From.ToShortDateString()} couldn't be served die to booking policy");
        }
    }
}
