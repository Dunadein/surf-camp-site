using System;

namespace SurfLevel.Contracts.Models.DatabaseObjects
{
    public class PayLog : IPrimaryKeyObject
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string RequestId { get; set; }
        public decimal EuroAmount { get; set; }
        public decimal Amount { get; set; }
        public decimal? FullAmount { get; set; }
        public DateTime Time { get; set; }
        public Direction Direction { get; set; }
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
