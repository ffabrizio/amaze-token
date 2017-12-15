using Amaze.Coin.Api;
using Amaze.Coin.Stores.Accounts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nethereum.HdWallet;
using Nethereum.Web3;

namespace Amaze.Coin.Tests
{
    [TestClass]
    public class AccountTests
    {
        AppSettings settings = new AppSettings
        {
            AdminSeed = "celery nuclear road south road balcony cabin practice velvet anger wrist ordinary",
            AdminPwd = "eAmazed17!",
            RpcEndpoint = "https://ropsten.infura.io"
        };

        [TestMethod]
        public void CreateAccount()
        {
            var account = UserAccount.Initialize("ffabrizio", "fabs");
            Assert.IsTrue(account.UserName == "ffabrizio");
            Assert.IsTrue(account.DisplayName == "fabs");
            Assert.IsNotNull(account.Wallet);
            Assert.AreEqual(12, account.Wallet.Words.Length);
            Assert.IsNotNull(account.Wallet.GetAccount(0).Address);
        }

        [TestMethod]
        public void CheckBalance()
        {
            var wallet = new Wallet(settings.AdminSeed, settings.AdminPwd);
            Assert.IsTrue(wallet.GetAccount(0).Address == "0x5003D65aCC048A2b2e8c0DF2FE501C02D284F790");
            var web3 = new Web3(wallet.GetAccount(0), settings.RpcEndpoint);
            var balance = web3.Eth.GetBalance.SendRequestAsync(wallet.GetAccount(0).Address).Result;
            var value = balance.Value;
            Assert.IsTrue(value > 0);
        }
    }
}
