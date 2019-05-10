using System.ComponentModel.DataAnnotations.Schema;

namespace SurfLevel.Contracts.Models.DatabaseObjects
{
    public class Guest : IPrimaryKeyObject
    {
        #region Columns
        [Column("g_Id")]
        public int Id { get; set; }
        [Column("g_oId")]
        public int OrderId { get; set; }
        [Column("g_Name")]
        public string Name { get; set; }
        [Column("g_Family")]
        public string LastName { get; set; }
        #endregion
    }
}
