using System.Threading.Tasks;
using Nethereum.RPC.Eth.DTOs;

namespace Amaze.Coin.Api.Interfaces
{
    public interface IAdminStore
    {
        Task<TransactionReceipt> GiveTokens(string toAddress, int tokens);
    }
}