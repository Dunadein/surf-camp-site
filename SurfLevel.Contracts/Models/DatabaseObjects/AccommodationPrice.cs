using System.ComponentModel.DataAnnotations.Schema;

namespace SurfLevel.Contracts.Models.DatabaseObjects
{
    public class AccommodationPrice : IPrimaryKeyObject
    {
        #region Columns
        [Column("ap_Id")]
        public int Id { get; set; }
        [Column("ap_rId")]
        public int RoomId { get; set; }
        [Column("ap_aId")]
        public int AccommodationId { get; set; }
        [Column("ap_DayPrice")]
        public decimal DayPrice { get; set; }
        #endregion

        public Accommodation Accommodation { get; set; }
    }
}
