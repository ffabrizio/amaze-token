using System;
using System.Collections.Generic;
using System.Linq;
using Amaze.Coin.Api.Accounts;
using Amaze.Coin.Api.Contracts;
using Nethereum.HdWallet;
using Nethereum.Web3;

namespace Amaze.Coin.Api.Stores
{
    public class AccountStore
    {
        private AppSettings AppSettings { get; set; }
        private AdminStore AdminStore { get; set; }
        
        // Our in-memory user DB, to be replaced by a persisted medium...
        private static readonly List<UserAccount> UserAccounts = new List<UserAccount>();

        public AccountStore(AppSettings settings, AdminStore adminStore)
        {
            AppSettings = settings;
            AdminStore = adminStore;
        }

        private static IEnumerable<UserAccount> GetAccounts()
        {
            return UserAccounts;
        }

        public static UserAccount GetAccount(string userName)
        {
            return GetAccounts().FirstOrDefault(_ => string.Equals(_.UserName, userName, StringComparison.InvariantCultureIgnoreCase));
        }

        public int GetBalance(Wallet wallet)
        {
            var web3 = new Web3(AppSettings.RpcEndpoint);
            var contractAddress = AppSettings.CoinContractAddress;
            var msg = new BalanceOfFunction
            {
                Owner = wallet.GetAccount(0).Address
            };
            
            var handler = web3.Eth.GetContractQueryHandler<BalanceOfFunction>();
            return handler.QueryAsync<int>(msg, contractAddress).Result;;
        }

        public UserAccount AddAccount(UserAccount account)
        {
            var existingAccount = GetAccount(account.UserName);
            if (existingAccount != null)
            {
                return existingAccount;
            }

            // Add to our in-memory DB
            UserAccounts.Add(account);

            var tx = AdminStore.CreditAddress(account.Wallet.GetAccount(0).Address, AppSettings.TokensOnAccountCreation);
            return GetAccount(account.UserName);
        }
    }
}
