using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amaze.Coin.Stores.Accounts;
using Nethereum.HdWallet;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;

namespace Amaze.Coin.Api.Stores
{
    public class AccountStore
    {
        private const string adminSeed = "celery nuclear road south road balcony cabin practice velvet anger wrist ordinary";
        private const string adminPwd = "eAmazed17!";
        private const string rpcEndpoint = "https://ropsten.infura.io";

        private static List<UserAccount> userAccounts = new List<UserAccount>();
        private Wallet adminWallet = new Wallet(adminSeed, adminPwd);

        public IEnumerable<UserAccount> GetAccounts()
        {
            return userAccounts;
        }

        public UserAccount GetAccount(string userName)
        {
            return GetAccounts().FirstOrDefault(_ => _.UserName.ToLowerInvariant() == userName.ToLowerInvariant());
        }

        public Wallet GetAdminWallet()
        {
            return adminWallet;
        }

        public int GetBalance(Wallet wallet)
        {
            var web3 = new Web3(wallet.GetAccount(0), rpcEndpoint);
            var balance = web3.Eth.GetBalance.SendRequestAsync(wallet.GetAccount(0).Address).Result;
            return (int)balance.Value;
        }

        public UserAccount AddAccount(UserAccount account)
        {
            var existingAccount = GetAccount(account.UserName);
            if (existingAccount != null)
            {
                return existingAccount;
            }

            userAccounts.Add(account);
            var tx = CreditSomeTokens(account.Wallet.GetAccount(0).Address);

            return GetAccount(account.UserName);
        }

        private async Task<TransactionReceipt> CreditSomeTokens(string address)
        {
            const int tokensToCredit = 20;
            var adminAccount = GetAdminWallet().GetAccount(0);
            var adminAddress = adminAccount.Address;

            var web3 = new Web3(adminAccount, rpcEndpoint);
            var adminBalance = web3.Eth.GetBalance.SendRequestAsync(adminAddress).Result;
            var transactionPolling = web3.TransactionManager.TransactionReceiptService;

            var transactionReceipt = await transactionPolling.SendRequestAsync(() =>
                web3.TransactionManager.SendTransactionAsync(adminAddress, address, new HexBigInteger(tokensToCredit))
            );

            return transactionReceipt;
        }
    }
}
