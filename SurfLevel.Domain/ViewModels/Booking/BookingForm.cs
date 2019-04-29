using SurfLevel.Contracts.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SurfLevel.Domain.ViewModels.Booking
{
    public class BookingForm : Request
    {
        public override void Validate()
        {
            if (Services == null)
                throw new ArgumentNullException("Not any picked service found.");

            if (Math.Max(Services.Count, Services.Sum(p => p.RoomId.HasValue ? p.Pax : 1)) < Pax)
                throw new ArgumentException("Not all the guests have any service picked.");

            if (Services.Count > Pax)
                throw new ArgumentException($"Too many services picked for amount of {Pax} guests.");
        }

        public string Email { get; set; }
        public string Phone { get; set; }
        public string Comment { get; set; }
        public string Name { get; set; }
        public string SecondName { get; set; }

        public List<PickedService> Services { get; set; }
    }
}
