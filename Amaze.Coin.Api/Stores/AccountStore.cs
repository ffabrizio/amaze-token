using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Amaze.Coin.Stores.Accounts;
using Nethereum.HdWallet;
using Nethereum.Web3;

namespace Amaze.Coin.Api.Stores
{
    public class AccountStore
    {
        private AppSettings AppSettings { get; set; }
        private AdminStore AdminStore { get; set; }
        private static List<UserAccount> userAccounts = new List<UserAccount>();

        public AccountStore(AppSettings settings, AdminStore adminStore)
        {
            AppSettings = settings;
            AdminStore = adminStore;
        }

        public IEnumerable<UserAccount> GetAccounts()
        {
            return userAccounts;
        }

        public UserAccount GetAccount(string userName)
        {
            return GetAccounts().FirstOrDefault(_ => _.UserName.ToLowerInvariant() == userName.ToLowerInvariant());
        }

        public BigInteger GetBalance(Wallet wallet)
        {
            var web3 = new Web3(wallet.GetAccount(0), AppSettings.RpcEndpoint);
            var balance = web3.Eth.GetBalance.SendRequestAsync(wallet.GetAccount(0).Address).Result;
            return balance.Value;
        }

        public UserAccount AddAccount(UserAccount account)
        {
            var existingAccount = GetAccount(account.UserName);
            if (existingAccount != null)
            {
                return existingAccount;
            }

            userAccounts.Add(account);

            var tx = AdminStore.CreditAddress(account.Wallet.GetAccount(0).Address, AppSettings.TokensOnAccountCreation);
            return GetAccount(account.UserName);
        }
    }
}
