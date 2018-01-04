using Amaze.Coin.Api.Models;

namespace Amaze.Coin.Api.Stores
{
    public interface IAccountStore
    {
        UserAccount GetAccount(string userName);
        UserAccount AddAccount(string userName);
        int GetBalance(string address);
    }
}