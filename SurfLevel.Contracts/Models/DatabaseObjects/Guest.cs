namespace SurfLevel.Contracts.Models.DatabaseObjects
{
    public class Guest : IPrimaryKeyObject
    {
        #region Columns
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        #endregion
    }
}
