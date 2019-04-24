namespace SurfLevel.Contracts.Models.DatabaseObjects
{
    public class PackagePeriodPrice
    {
        #region Columns
        public int Id { get; set; }
        public int PackageId { get; set; }
        public int PeriodStart { get; set; }
        public int? PeriodEnd { get; set; }
        public decimal Price { get; set; }
        #endregion

        //public virtual Package Package { get; set; }
    }
}
