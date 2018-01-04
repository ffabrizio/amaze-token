using Amaze.Coin.Api;
using Amaze.Coin.Api.Contracts;
using Amaze.Coin.Api.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System.IO;

namespace Amaze.Coin.Tests
{
    [TestClass]
    public class AccountTests
    {
        private readonly AppSettings _settings = new AppSettings
        {
            RpcEndpoint = "https://ropsten.infura.io",
            CoinContractAddress = "0x98B4ca39273c6378F803AD917FAdA45FD97Ce192",
        };


        [TestMethod]
        public void new_account_should_have_hdwallet_available()
        {
            var account = UserAccount.Initialize("ffabrizio", "fabs");
            Assert.IsTrue(account.UserName == "ffabrizio");
            Assert.IsTrue(account.DisplayName == "fabs");
            Assert.IsNotNull(account.Wallet);
            Assert.AreEqual(12, account.Wallet.Words.Length);
            Assert.IsNotNull(account.Wallet.GetAccount(0).Address);
        }

        [TestMethod]
        public void admin_account_should_have_ether_available()
        {
            //var keyRing = new DirectoryInfo(Path.Combine(env.WebRootPath, "Keys"));
            const string privateKey = "0xd7c66a2a58ed5100fa15507228f3225397c544c4f227a40117a2d900d11368f1";
            var account = new Account(privateKey);
            
            Assert.IsTrue(account.Address == "0x675bb916858f96746d48A98Fc9cEf49d9a37a5D9");
            
            var web3 = new Web3(account, _settings.RpcEndpoint);
            var balance = web3.Eth.GetBalance.SendRequestAsync(account.Address).Result;
            var value = balance.Value;
            
            Assert.IsTrue(value > 0);
        }
        
        [TestMethod]
        public void admin_account_should_have_tokens_available()
        {
            var web3 = new Web3(_settings.RpcEndpoint);
            var msg = new BalanceOfFunction
            {
                Owner = "0x675bb916858f96746d48a98fc9cef49d9a37a5d9"
            };
            var handler = web3.Eth.GetContractQueryHandler<BalanceOfFunction>();

            var contractAddress = _settings.CoinContractAddress;
            var balance = handler.QueryAsync<int>(msg, contractAddress).Result;

            Assert.IsTrue(balance > 0);
        }
        
        [TestMethod]
        public void admin_account_could_give_tokens_to_hdwallets()
        {
            const string privateKey = "0xd7c66a2a58ed5100fa15507228f3225397c544c4f227a40117a2d900d11368f1";
            var account = new Account(privateKey);
            var web3 = new Web3(account, _settings.RpcEndpoint);
            var msg = new TransferFunction
            {
                FromAddress = account.Address,
                To = "0x5ef8BA454A4295288e82dE67AB7f4559539664C3",
                TokenAmount = 5
            };
            
            var contractAddress = _settings.CoinContractAddress;

            var handler = web3.Eth.GetContractTrasactionHandler<TransferFunction>();
            var transactionHash = handler.SendRequestAsync(msg, contractAddress).Result;
            
            Assert.IsTrue(transactionHash != null);
        }
    }
}
