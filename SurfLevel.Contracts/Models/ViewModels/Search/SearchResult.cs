using SurfLevel.Contracts.Models.ViewModels.Packages;
using System.Collections.Generic;

namespace SurfLevel.Contracts.Models.ViewModels.Search
{
    public class SearchPackagesResult
    {
        public SearchPackagesResult()
        {
            Packages = new List<ViewPackageWithId>();
        }

        public List<ViewPackageWithId> Packages { get; set; }
    }
}
