using System.Security.Principal;
using Amaze.Coin.Api.Accounts;
using Amaze.Coin.Api.Stores;
using Amaze.Coin.Models;
using Microsoft.AspNetCore.Mvc;

namespace Amaze.Coin.Controllers
{
    public class HomeController : Controller
    {
        private AccountStore AccountStore { get; }
        
        public HomeController(AccountStore accountStore)
        {
            AccountStore = accountStore;
        }

        public IActionResult Index()
        {
            var user = new GenericIdentity("ffabrizio");
            var account = AccountStore.GetAccount(user.Name);
            var vm = new AppVm();

            if (account == null)
            {
                account = AccountStore.AddAccount(UserAccount.Initialize(user.Name));
                vm.IsNewAccount = true;
            }

            vm.Wallet = account.Wallet;
            vm.Balance = AccountStore.GetBalance(account.Wallet);

            return View(vm);
        }

        public IActionResult CheckBalance()
        {
            var user = new GenericIdentity("ffabrizio");
            var account = AccountStore.GetAccount(user.Name);
            var balance = AccountStore.GetBalance(account.Wallet);

            return new JsonResult(balance);
        }
    }
}