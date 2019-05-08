namespace SurfLevel.Contracts.Models.DatabaseObjects
{
    public class PackagePeriodPrice : IPrimaryKeyObject
    {
        #region Columns
        public int Id { get; set; }
        public int PackageId { get; set; }
        public int PeriodStart { get; set; }
        public int? PeriodEnd { get; set; }
        public decimal Price { get; set; }
        #endregion
    }
}
