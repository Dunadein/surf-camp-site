namespace SurfLevel.Contracts.Interfaces.Services
{
    public interface IHasherService<TRequest>
    {
        string Create(TRequest request);

        TRequest Read(string hash);
    }
}
