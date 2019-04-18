using System.Collections.Generic;

namespace SurfLevel.Contracts.Models.ViewModels.Search
{
    public class SearchPriceByPackageResult
    {
        public List<PaxPrice> Prices { get; set; }

        public List<ViewVilla> Villas { get; set; }
    }
}
