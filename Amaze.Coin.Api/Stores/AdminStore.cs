using Nethereum.HdWallet;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using System.Threading.Tasks;

namespace Amaze.Coin.Api.Stores
{
    public class AdminStore
    {
        private AppSettings AppSettings { get; set; }
        private Wallet adminWallet;

        public AdminStore(AppSettings settings)
        {
            AppSettings = settings;
            adminWallet = new Wallet(AppSettings.AdminSeed, AppSettings.AdminPwd);
        }

        internal async Task<TransactionReceipt> CreditAddress(string address, int number)
        {
            var adminAccount = adminWallet.GetAccount(0);
            var adminAddress = adminAccount.Address;

            var web3 = new Web3(adminAccount, AppSettings.RpcEndpoint);
            var adminBalance = web3.Eth.GetBalance.SendRequestAsync(adminAddress).Result;
            var transactionPolling = web3.TransactionManager.TransactionReceiptService;

            var transactionReceipt = await transactionPolling.SendRequestAsync(() =>
                web3.TransactionManager.SendTransactionAsync(adminAddress, address, new HexBigInteger(AppSettings.TokensOnAccountCreation))
            );

            return transactionReceipt;
        }
    }
}
