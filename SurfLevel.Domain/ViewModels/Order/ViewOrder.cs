using SurfLevel.Contracts.Models.DatabaseObjects;
using SurfLevel.Contracts.Models.DTO;
using System.Collections.Generic;

namespace SurfLevel.Domain.ViewModels.Order
{
    public class ViewOrder
    {
        public string OrderId { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string FullName { get; set; }
        public string From { get; set; }
        public string Till { get; set; }
        public int Pax { get; set; }
        public decimal FullPrice { get; set; }        
        public PrepaymentInfo Prepayment { get; set; }
        public OrderStatus Status { get; set; }        

        public IEnumerable<ViewGuest> Guests { get; set; }
        public IEnumerable<ViewService> Services { get; set; }
    }
}
