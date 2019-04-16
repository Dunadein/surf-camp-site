using SurfLevel.Contracts.ViewModels.Search;
using System.Threading.Tasks;

namespace SurfLevel.Contracts.Interfaces.Services
{
    public interface ISearchProvider
    {
        Task<SearchResult> SearchAvailableResources(SearchRequest request);
    }
}
