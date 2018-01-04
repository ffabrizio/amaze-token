using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Amaze.Coin.Api.Contracts;
using Amaze.Coin.Api.Interfaces;
using Amaze.Coin.Api.Models;
using Nethereum.Web3;

namespace Amaze.Coin.Api.Stores
{
    public class AccountStore : IAccountStore
    {
        private AppSettings AppSettings { get; }
        private IAdminStore AdminStore { get; }
        
        // Our in-memory user DB, to be replaced by a persisted medium...
        private static readonly List<UserAccount> UserAccounts = new List<UserAccount>();

        public AccountStore(AppSettings settings, IAdminStore adminStore)
        {
            AppSettings = settings;
            AdminStore = adminStore;
        }

        private static IEnumerable<UserAccount> GetAccounts()
        {
            return UserAccounts;
        }

        public UserAccount GetAccount(string userName)
        {
            return GetAccounts().FirstOrDefault(_ => string.Equals(_.UserName, userName, StringComparison.InvariantCultureIgnoreCase));
        }

        public int GetBalance(string address)
        {
            var web3 = new Web3(AppSettings.RpcEndpoint);
            var contractAddress = AppSettings.CoinContractAddress;
            var msg = new BalanceOfFunction
            {
                Owner = address
            };
            
            var handler = web3.Eth.GetContractQueryHandler<BalanceOfFunction>();
            return handler.QueryAsync<int>(msg, contractAddress).Result;
        }

        public UserAccount AddAccount(string userName)
        {
            var existingAccount = GetAccount(userName);
            if (existingAccount != null)
            {
                return existingAccount;
            }

            // Add to our in-memory DB
            var account = UserAccount.Initialize(userName);
            UserAccounts.Add(account);

            var tx = AdminStore.GiveTokens(account.Wallet.GetAccount(0).Address, AppSettings.StartupTokens).Result;
            Debug.Write(tx.TransactionHash);
            
            return GetAccount(account.UserName);
        }
    }
}
