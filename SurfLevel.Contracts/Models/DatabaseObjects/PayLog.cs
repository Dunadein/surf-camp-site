using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SurfLevel.Contracts.Models.DatabaseObjects
{
    public class PayLog : IPrimaryKeyObject
    {
        [Column("pl_Id")]
        public int Id { get; set; }
        [Column("pl_oId")]
        public int OrderId { get; set; }
        [Column("pl_RequestId")]
        public string RequestId { get; set; }
        [Column("pl_EuroAmount")]
        public decimal EuroAmount { get; set; }
        [Column("pl_ToPay")]
        public decimal IncomeAmount { get; set; }
        [Column("pl_Amount")]
        public decimal? Amount { get; set; }
        [Column("pl_TimeMark")]
        public DateTime Time { get; set; }
        [Column("pl_Direction")]
        public Direction Direction { get; set; }
        [Column("pl_Status")]
        public Status Status { get; set; }
    }

    public enum Direction
    {
        Out,
        In
    }

    public enum Status
    {
        Open,
        Closed
    }
}
