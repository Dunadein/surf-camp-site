using SurfLevel.Contracts.Models.DTO;
using System;

namespace SurfLevel.Domain.ViewModels.Search
{
    public class SearchRequest : Request
    {
        public bool WithAccommodation { get; set; } 

        public override void Validate()
        {
            if (WithAccommodation && !Till.HasValue)
                throw new ArgumentException("End date wasn't provide");

            base.ValidateBasic();
        }
    }
}
