using System.Collections.Generic;

namespace SurfLevel.Contracts.ViewModels.Search
{
    public class SearchResult
    {
        public SearchResult()
        {
            Packages = new List<ViewPackage>();
        }

        public List<ViewPackage> Packages { get; set; }
    }
}
