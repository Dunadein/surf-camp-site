using System.Collections.Generic;

namespace SurfLevel.Contracts.Models.DatabaseObjects
{
    public class Package
    {
        public Package()
        {
            OutOfServicePeriods = new HashSet<OutOfServicePeriod>();
        }

        #region Columns 
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortLabel { get; set; }
        public decimal? PromoPrice { get; set; }
        public decimal MinDayPrice { get; set; }
        public decimal Percent { get; set; }
        public bool IsWithAccommodation { get; set; }
        public bool IsDefault { get; set; }
        #endregion

        public virtual ICollection<OutOfServicePeriod> OutOfServicePeriods { get; set; }
    }
}
