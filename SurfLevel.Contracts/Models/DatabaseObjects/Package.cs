namespace SurfLevel.Contracts.Models.DatabaseObjects
{
    public class Package
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortLabel { get; set; }
        public decimal? PromoPrice { get; set; }
        public decimal MinDayPrice { get; set; }
        public decimal Percent { get; set; }
        public bool IsWithAccommodation { get; set; }
    }
}
