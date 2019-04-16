namespace SurfLevel.Contracts.Models.DatabaseObjects
{
    public class AccommodationPrice
    {
        #region Columns
        public int Id { get; set; }
        public int RoomId { get; set; }
        public int AccommodationId { get; set; }
        public decimal Price { get; set; }
        #endregion

        public Accommodation Accommodation { get; set; }
    }
}
