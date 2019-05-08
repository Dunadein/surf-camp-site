using System.Collections.Generic;

namespace SurfLevel.Domain.ViewModels.Order
{
    public class PrepaymentInfo
    {
        public List<PrepayType> AvailableType { get; set; }
        public string Amount { get; set; }
        public decimal Payed { get; set; }
    }

    public enum PrepayType
    {
        FullPayment = 0,
        PerPerson = 1,
        PerService = 2
    }
}
