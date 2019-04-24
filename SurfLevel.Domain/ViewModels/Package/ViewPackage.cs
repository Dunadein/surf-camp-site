namespace SurfLevel.Domain.ViewModels.Package
{
    public class ViewPackage
    {
        public string Label { get; set; } 
        public decimal? Price { get; set; }
        public string LocaleFolder { get; set; }
        public bool IsDefault { get; set; }
    }
}
