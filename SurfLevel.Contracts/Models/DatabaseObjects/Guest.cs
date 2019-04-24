namespace SurfLevel.Contracts.Models.DatabaseObjects
{
    public class Guest
    {
        #region Columns
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public int PackageId { get; set; }
        public int? AccommodationPriceId { get; set; }
        public int? ServiceDays { get; set; }
        public decimal? PrepayPercent { get; set; }        
        public decimal Price { get; set; }
        #endregion

        public Package Package { get; set; }
        public AccommodationPrice AccommodationPrice { get; set; }
    }
}
