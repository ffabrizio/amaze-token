using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using System.Threading.Tasks;
using Amaze.Coin.Api.Contracts;
using Nethereum.Web3.Accounts;

namespace Amaze.Coin.Api.Stores
{
    public class AdminStore
    {
        private AppSettings AppSettings { get; set; }
        private readonly Account _adminAccount;

        public AdminStore(AppSettings settings)
        {
            AppSettings = settings;
            _adminAccount = new Account(AppSettings.AdminKey);
        }

        internal async Task<TransactionReceipt> CreditAddress(string address, int tokens = 0)
        {
            if (tokens <= 0) return null;
            
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
            return await handler.SendRequestAsync(msg, contractAddress);
        }
    }
}
