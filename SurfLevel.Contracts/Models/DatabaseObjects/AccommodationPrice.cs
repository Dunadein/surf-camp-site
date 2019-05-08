namespace SurfLevel.Contracts.Models.DatabaseObjects
{
    public class AccommodationPrice : IPrimaryKeyObject
    {
        #region Columns
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int AccommodationId { get; set; }
        public decimal DayPrice { get; set; }
        #endregion

        public Accommodation Accommodation { get; set; }
    }
}
