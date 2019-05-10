using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurfLevel.Contracts.Models.DatabaseObjects
{
    public class Order : IPrimaryKeyObject
    {
        public Order()
        {
            Guests = new HashSet<Guest>();
            Services = new HashSet<Service>();
        }

        #region Columns
        [Column("o_Id")]
        public int Id { get; set; }
        [Column("o_Created")]
        public DateTime Created { get; set; }
        [Column("o_Hash")]
        public string HashKey { get; set; }
        [Column("o_ContactEmail")]
        public string ContactEmail { get; set; }
        [Column("o_ContactPhone")]
        public string ContactPhone { get; set; }
        [Column("o_GuestName")]
        public string GuestName { get; set; }
        [Column("o_GuestFamily")]
        public string GuestSecondName { get; set; }
        [Column("o_DateFrom")]
        public DateTime DateFrom { get; set; }
        [Column("o_DateTill")]
        public DateTime? DateTill { get; set; }
        [Column("o_GuestsCount")]
        public int GuestsCount { get; set; }
        [Column("o_Price")]
        public decimal TotalPrice { get; set; }
        [Column("o_Status")]
        public OrderStatus Status { get; set; }
        [Column("o_Comment")]
        public string Comment { get; set; }
        [Column("o_Locale")]
        public string Locale { get; set; }
        [Column("o_Payed")]
        public decimal? Payed { get; set; }
        [Column("o_Commission")]
        public bool IsCommission { get; set; }
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
