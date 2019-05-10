using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurfLevel.Contracts.Models.DatabaseObjects
{
    public class OutOfServicePeriod : IPrimaryKeyObject
    {
        [Column("sp_Id")]
        public int Id { get; set; }
        [Column("sp_Id")]
        public int PackageId { get; set; }
        [Column("sp_Start")]
        public DateTime Start { get; set; }
        [Column("sp_End")]
        public DateTime End { get; set; }
    }
}
