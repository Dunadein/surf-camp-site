using SurfLevel.Contracts.Models.DTO;
using System.Collections.Generic;

namespace SurfLevel.Domain.ViewModels.Search
{
    public class ViewRoom
    {
        public ViewRoom()
        {
            Prices = new List<PaxPrice>();
        }
        public int Id { get; set; }
        public string Folder { get; set; }
        public List<PaxPrice> Prices { get; set; }
        public int MaxPax { get; set; }
    }
}
