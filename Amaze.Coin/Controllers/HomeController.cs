using System.Security.Principal;
using Amaze.Coin.Api.Stores;
using Amaze.Coin.Models;
using Amaze.Coin.Stores.Accounts;
using Microsoft.AspNetCore.Mvc;

namespace Amaze.Coin.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var user = (WindowsIdentity)User.Identity;
            var store = new AccountStore();
            var account = store.GetAccount(user.Name);
            var vm = new AppVm();

            if (account == null)
            {
                account = store.AddAccount(UserAccount.Initialize(user.Name));
                vm.IsNewAccount = true;
            }

            vm.Wallet = account.Wallet;
            vm.Balance = store.GetBalance(account.Wallet);

            return View(vm);
        }

        public IActionResult CheckBalance()
        {
            var user = (WindowsIdentity)User.Identity;
            var store = new AccountStore();
            var account = store.GetAccount(user.Name);
            var balance = store.GetBalance(account.Wallet);

            return new JsonResult(balance);
        }
    }
}