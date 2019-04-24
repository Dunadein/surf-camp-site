using System;
using System.Collections.Generic;

namespace SurfLevel.Contracts.Models.DTO
{
    public class DateWithGroupingByRoom
    {
        public DateTime Date { get; set; }
        public List<GroupTypePax> Groupping { get; set; }
    }
}
