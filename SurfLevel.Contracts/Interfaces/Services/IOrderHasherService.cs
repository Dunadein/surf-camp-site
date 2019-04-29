namespace SurfLevel.Contracts.Interfaces.Services
{
    public interface IOrderHasherService
    {
        string CreateOrderHash(string hashKey);

        string ReadOrderHash(string hashedOrder, bool supportInherited = true);
    }
}
