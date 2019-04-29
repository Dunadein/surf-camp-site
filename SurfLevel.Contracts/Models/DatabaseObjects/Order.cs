using System;
using System.Collections.Generic;

namespace SurfLevel.Contracts.Models.DatabaseObjects
{
    public class Order
    {
        public Order()
        {
            Guests = new HashSet<Guest>();
            Services = new HashSet<Service>();
        }

        #region Columns
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public string HashKey { get; set; }
        public string ContactEmail { get; set; }
        public string ContactPhone { get; set; }
        public string GuestName { get; set; }
        public string GuestSecondName { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime? DateTill { get; set; }
        public int GuestsCount { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderStatus Status { get; set; }
        public string Comment { get; set; }
        #endregion

        public virtual ICollection<Guest> Guests { get; set; }
        public virtual ICollection<Service> Services { get; set; }
    }

    public enum OrderStatus
    {
        Request = 0,
        Confirmed = 1,
        PartlyPayed = 2,
        Payed = 3,
        Annulated = 5
    }
}
