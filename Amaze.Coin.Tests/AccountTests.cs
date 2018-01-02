using System.Numerics;
using Amaze.Coin.Api;
using Amaze.Coin.Api.Accounts;
using Amaze.Coin.Api.Contracts;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NBitcoin;
using Nethereum.HdWallet;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;

namespace Amaze.Coin.Tests
{
    [TestClass]
    public class AccountTests
    {
        readonly AppSettings _settings = new AppSettings
        {
            RpcEndpoint = "https://ropsten.infura.io",
            CoinContractAddress = "0x5ea419d90f8babe5299be43363c2a6f15f69b5ce",
            Abi = "[{\"constant\":true,\"inputs\":[],\"name\":\"name\",\"outputs\":[{\"name\":\"\",\"type\":\"string\"}],\"payable\":false,\"stateMutability\":\"view\",\"type\":\"function\"},{\"constant\":false,\"inputs\":[{\"name\":\"_spender\",\"type\":\"address\"},{\"name\":\"_value\",\"type\":\"uint256\"}],\"name\":\"approve\",\"outputs\":[{\"name\":\"\",\"type\":\"bool\"}],\"payable\":false,\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"constant\":true,\"inputs\":[],\"name\":\"totalSupply\",\"outputs\":[{\"name\":\"\",\"type\":\"uint256\"}],\"payable\":false,\"stateMutability\":\"view\",\"type\":\"function\"},{\"constant\":false,\"inputs\":[{\"name\":\"_from\",\"type\":\"address\"},{\"name\":\"_to\",\"type\":\"address\"},{\"name\":\"_value\",\"type\":\"uint256\"}],\"name\":\"transferFrom\",\"outputs\":[{\"name\":\"\",\"type\":\"bool\"}],\"payable\":false,\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"constant\":true,\"inputs\":[],\"name\":\"INITIAL_SUPPLY\",\"outputs\":[{\"name\":\"\",\"type\":\"uint256\"}],\"payable\":false,\"stateMutability\":\"view\",\"type\":\"function\"},{\"constant\":true,\"inputs\":[],\"name\":\"decimals\",\"outputs\":[{\"name\":\"\",\"type\":\"uint256\"}],\"payable\":false,\"stateMutability\":\"view\",\"type\":\"function\"},{\"constant\":false,\"inputs\":[{\"name\":\"_spender\",\"type\":\"address\"},{\"name\":\"_subtractedValue\",\"type\":\"uint256\"}],\"name\":\"decreaseApproval\",\"outputs\":[{\"name\":\"\",\"type\":\"bool\"}],\"payable\":false,\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"constant\":true,\"inputs\":[{\"name\":\"_owner\",\"type\":\"address\"}],\"name\":\"balanceOf\",\"outputs\":[{\"name\":\"balance\",\"type\":\"uint256\"}],\"payable\":false,\"stateMutability\":\"view\",\"type\":\"function\"},{\"constant\":true,\"inputs\":[],\"name\":\"symbol\",\"outputs\":[{\"name\":\"\",\"type\":\"string\"}],\"payable\":false,\"stateMutability\":\"view\",\"type\":\"function\"},{\"constant\":false,\"inputs\":[{\"name\":\"_to\",\"type\":\"address\"},{\"name\":\"_value\",\"type\":\"uint256\"}],\"name\":\"transfer\",\"outputs\":[{\"name\":\"\",\"type\":\"bool\"}],\"payable\":false,\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"constant\":false,\"inputs\":[{\"name\":\"_spender\",\"type\":\"address\"},{\"name\":\"_addedValue\",\"type\":\"uint256\"}],\"name\":\"increaseApproval\",\"outputs\":[{\"name\":\"\",\"type\":\"bool\"}],\"payable\":false,\"stateMutability\":\"nonpayable\",\"type\":\"function\"},{\"constant\":true,\"inputs\":[{\"name\":\"_owner\",\"type\":\"address\"},{\"name\":\"_spender\",\"type\":\"address\"}],\"name\":\"allowance\",\"outputs\":[{\"name\":\"\",\"type\":\"uint256\"}],\"payable\":false,\"stateMutability\":\"view\",\"type\":\"function\"},{\"inputs\":[],\"payable\":false,\"stateMutability\":\"nonpayable\",\"type\":\"constructor\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"name\":\"owner\",\"type\":\"address\"},{\"indexed\":true,\"name\":\"spender\",\"type\":\"address\"},{\"indexed\":false,\"name\":\"value\",\"type\":\"uint256\"}],\"name\":\"Approval\",\"type\":\"event\"},{\"anonymous\":false,\"inputs\":[{\"indexed\":true,\"name\":\"from\",\"type\":\"address\"},{\"indexed\":true,\"name\":\"to\",\"type\":\"address\"},{\"indexed\":false,\"name\":\"value\",\"type\":\"uint256\"}],\"name\":\"Transfer\",\"type\":\"event\"}]"
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
            const string privateKey = "0xd7c66a2a58ed5100fa15507228f3225397c544c4f227a40117a2d900d11368f1";
            var account = new Account(privateKey);
            
            Assert.IsTrue(account.Address == "0x675bb916858f96746d48A98Fc9cEf49d9a37a5D9");
            
            var web3 = new Web3(account, _settings.RpcEndpoint);
            var balance = web3.Eth.GetBalance.SendRequestAsync(account.Address).Result;
            var value = balance.Value;
            
            Assert.IsTrue(value > 0);
        }
        
        [TestMethod]
        public void CheckAmzBalance()
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
        public void Transfer()
        {
            const string privateKey = "0xd7c66a2a58ed5100fa15507228f3225397c544c4f227a40117a2d900d11368f1";
            var account = new Account(privateKey);
            var web3 = new Web3(account, _settings.RpcEndpoint);
            var msg = new TransferFunction
            {
                FromAddress = account.Address,
                To = "0xe875f2850f828916b924c10fd056Defb15f449c9",
                TokenAmount = 1
            };
            
            var contractAddress = _settings.CoinContractAddress;

            var handler = web3.Eth.GetContractTrasactionHandler<TransferFunction>();
            var transactionHash = handler.SendRequestAsync(msg, contractAddress).Result;
            
            Assert.IsTrue(transactionHash != null);
        }
    }
}
