using System.Threading.Tasks;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Nethereum.RPC.Eth.DTOs;
using Amaze.Coin.Api.Contracts;
using Amaze.Coin.Api.Services;

namespace Amaze.Coin.Api.Stores
{
    public class AdminStore : IAdminStore
    {
        private AppSettings AppSettings { get; }

        private readonly Account _adminAccount;

        public AdminStore(AppSettings settings, ICipherService cipherService)
        {
            AppSettings = settings;
            _adminAccount = new Account(cipherService.Decrypt(AppSettings.AdminKey));
        }

        public async Task<TransactionReceipt> GiveTokens(string address, int tokens)
        {
            if (tokens <= 0) return null;
            if (_adminAccount == null) return null;
            
            var adminAddress = _adminAccount.Address;

            var web3 = new Web3(_adminAccount, AppSettings.RpcEndpoint);
            var contractAddress = AppSettings.CoinContractAddress;
            
            var msg = new TransferFunction
            {
                FromAddress = adminAddress,
                To = address,
                TokenAmount = tokens
            };
            
            var handler = web3.Eth.GetContractTrasactionHandler<TransferFunction>();
            var tx = await handler.SendRequestAsync(msg, contractAddress);
            return new TransactionReceipt { TransactionHash = tx };
        }
    }
}
