using System;

namespace SurfLevel.Contracts.Models.DatabaseObjects
{
    public class OutOfServicePeriod
    {
        public int Id { get; set; }
        public int PackageId { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
