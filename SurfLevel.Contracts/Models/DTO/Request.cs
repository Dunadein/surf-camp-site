using System;

namespace SurfLevel.Contracts.Models.DTO
{
    public abstract class Request
    {
        public DateTime From { get; set; }
        public DateTime? Till { get; set; }
        public int Pax { get; set; }

        public abstract void Validate();

        protected void ValidateBasic()
        {
            if (DateTime.UtcNow.Date >= From.Date)
                throw new ArgumentException($"The From Date of value {From.ToShortDateString()} couldn't be served die to booking policy");
        }
    }
}
