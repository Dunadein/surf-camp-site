using System.ComponentModel.DataAnnotations.Schema;

namespace SurfLevel.Contracts.Models.DatabaseObjects
{
    public class Service : IPrimaryKeyObject
    {
        [Column("s_Id")]
        public int Id { get; set; }
        [Column("s_oId")]
        public int OrderId { get; set; }
        [Column("s_pId")]
        public int PackageId { get; set; }
        [Column("s_apId")]
        public int? AccommodationPriceId { get; set; }
        [Column("s_Days")]
        public int? ServiceDays { get; set; }
        [Column("s_Price")]
        public decimal Price { get; set; }

        public Package Package { get; set; }
        public AccommodationPrice AccommodationPrice { get; set; }
    }
}
