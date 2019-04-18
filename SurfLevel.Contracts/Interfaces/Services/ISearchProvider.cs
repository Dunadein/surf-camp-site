using SurfLevel.Contracts.Models.ViewModels.Search;
using System.Threading.Tasks;

namespace SurfLevel.Contracts.Interfaces.Services
{
    public interface ISearchProvider
    {
        Task<string> GetHashedRequest(SearchRequest request);

        Task<SearchPackagesResult> SearchAvailablePackages(string hash);
    }
}
