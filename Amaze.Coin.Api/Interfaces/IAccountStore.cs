using Amaze.Coin.Api.Models;

namespace Amaze.Coin.Api.Interfaces
{
    public interface IAccountStore
    {
        UserAccount GetAccount(string userName);
        UserAccount AddAccount(string userName);
        int GetBalance(string address);
    }
}