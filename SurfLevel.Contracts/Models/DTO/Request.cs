using System;

namespace SurfLevel.Contracts.Models.DTO
{
    public abstract class Request
    {
        public DateTime From { get; set; }
        public DateTime? Till { get; set; }
        public int Pax { get; set; }
        public bool WithAccommodation { get; set; }

        public abstract void Validate();
    }
}
