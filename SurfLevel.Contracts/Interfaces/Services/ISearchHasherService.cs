using SurfLevel.Contracts.Models.DTO;

namespace SurfLevel.Contracts.Interfaces.Services
{
    public interface ISearchHasherService
    {
        string Create<TRequest>(TRequest request) where TRequest : Request;

        TRequest Read<TRequest>(string hash) where TRequest : Request, new();
    }
}
