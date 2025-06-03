namespace LiveLib.Application.Interfaces
{
    public interface IPassowrdHasher
    {
        string Hash(string password);

        bool Verify(string password, string hash);
    }
}
