using System.ComponentModel.DataAnnotations.Schema;

namespace SurfLevel.Contracts.Models.DatabaseObjects
{
    public class Accommodation : IPrimaryKeyObject
    {
        [Column("a_Id")]
        public int Id { get; set; }
        [Column("a_Name")]
        public string Name { get; set; }
        [Column("a_ShortCode")]
        public string ShortCode { get; set; }
        [Column("a_Capacity")]
        public int Capacity { get; set; }
    }
}
