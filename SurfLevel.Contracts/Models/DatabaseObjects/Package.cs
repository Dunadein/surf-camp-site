using System.Collections.Generic;

namespace SurfLevel.Contracts.Models.DatabaseObjects
{
    public class Package : IPrimaryKeyObject
    {
        public Package()
        {
            OutOfServicePeriods = new HashSet<OutOfServicePeriod>();
            PackagePrices = new HashSet<PackagePeriodPrice>();
        }

        #region Columns 
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortLabel { get; set; }
        public decimal MinDayPrice { get; set; }
        public decimal Percent { get; set; }
        public bool IsWithAccommodation { get; set; }
        public bool IsDefault { get; set; }
        #endregion

        public virtual ICollection<OutOfServicePeriod> OutOfServicePeriods { get; set; }
        public virtual ICollection<PackagePeriodPrice> PackagePrices { get; set; }
    }
}
