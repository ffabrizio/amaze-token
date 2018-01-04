namespace Amaze.Coin.Api.Interfaces
{
    public interface ICipherService
    {
        string Encrypt(string input);
        string Decrypt(string cipherText);
    }
}