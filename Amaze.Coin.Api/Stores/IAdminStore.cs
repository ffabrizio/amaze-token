using System.Threading.Tasks;
using Nethereum.RPC.Eth.DTOs;

namespace Amaze.Coin.Api.Stores
{
    public interface IAdminStore
    {
        Task<TransactionReceipt> GiveTokens(string toAddress, int tokens);
    }
}