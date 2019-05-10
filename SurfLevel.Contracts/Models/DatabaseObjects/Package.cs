using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Column("p_Id")]
        public int Id { get; set; }
        [Column("p_Name")]
        public string Name { get; set; }
        [Column("p_ShortLabel")]
        public string ShortLabel { get; set; }
        [Column("p_MinDayPrice")]
        public decimal MinDayPrice { get; set; }
        [Column("p_Percent")]
        public decimal Percent { get; set; }
        [Column("p_IsWithAcc")]
        public bool IsWithAccommodation { get; set; }
        [Column("p_Default")]
        public bool IsDefault { get; set; }
        #endregion

        public virtual ICollection<OutOfServicePeriod> OutOfServicePeriods { get; set; }
        public virtual ICollection<PackagePeriodPrice> PackagePrices { get; set; }
    }
}
