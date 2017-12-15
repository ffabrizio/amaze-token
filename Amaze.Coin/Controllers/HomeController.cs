using System.Security.Principal;
using Amaze.Coin.Api.Stores;
using Amaze.Coin.Models;
using Amaze.Coin.Stores.Accounts;
using Microsoft.AspNetCore.Mvc;

namespace Amaze.Coin.Controllers
{
    public class HomeController : Controller
    {
        public AccountStore AccountStore { get; private set; }
        public HomeController(AccountStore accountStore)
        {
            AccountStore = accountStore;
        }

        public IActionResult Index()
        {
            var user = (WindowsIdentity)User.Identity;
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
            var user = (WindowsIdentity)User.Identity;
            var account = AccountStore.GetAccount(user.Name);
            var balance = AccountStore.GetBalance(account.Wallet);

            return new JsonResult(balance);
        }
    }
}