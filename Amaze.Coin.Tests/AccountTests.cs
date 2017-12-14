using Amaze.Coin.Api.Stores;
using Amaze.Coin.Stores.Accounts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Amaze.Coin.Tests
{
    [TestClass]
    public class AccountTests
    {
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
        public void StoreAccount()
        {
            var account = UserAccount.Initialize("ffabrizio", "fabs");
            var store = new AccountStore();

            var storedAccount = store.AddAccount(account);
            var retrievedAccount = store.GetAccount("ffabrizio");

            Assert.AreEqual(storedAccount.Wallet.Seed, retrievedAccount.Wallet.Seed);
        }
    }
}
