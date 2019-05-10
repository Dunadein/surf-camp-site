using System.ComponentModel.DataAnnotations.Schema;

namespace SurfLevel.Contracts.Models.DatabaseObjects
{
    public class PackagePeriodPrice : IPrimaryKeyObject
    {
        #region Columns
        [Column("pp_Id")]
        public int Id { get; set; }
        [Column("pp_pId")]
        public int PackageId { get; set; }
        [Column("pp_From")]
        public int PeriodFrom { get; set; }
        [Column("pp_Till")]
        public int? PeriodTill { get; set; }
        [Column("pp_Price")]
        public decimal Price { get; set; }
        #endregion
    }
}
