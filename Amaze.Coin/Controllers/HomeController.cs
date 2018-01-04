using System;
using System.Security.Principal;
using Microsoft.AspNetCore.Mvc;
using Amaze.Coin.Api.Stores;
using Amaze.Coin.Models;

namespace Amaze.Coin.Controllers
{
    public class HomeController : Controller
    {
        private IAccountStore AccountStore { get; }
        
        public HomeController(IAccountStore accountStore)
        {
            AccountStore = accountStore;
        }

        public IActionResult Index()
        {
            var user = GetCurrentUser();
            var account = AccountStore.GetAccount(user.Name);
            var vm = new AppVm();

            if (account == null)
            {
                account = AccountStore.AddAccount(user.Name);
                vm.IsNewAccount = true;
            }

            vm.Wallet = account.Wallet;
            vm.Balance = AccountStore.GetBalance(account.Wallet.GetAccount(0).Address);

            return View(vm);
        }

        public IActionResult CheckBalance()
        {
            var user = GetCurrentUser();
            var account = AccountStore.GetAccount(user.Name);
            var balance = AccountStore.GetBalance(account.Wallet.GetAccount(0).Address);

            return new JsonResult(balance);
        }

        private IIdentity GetCurrentUser()
        {
            // POC - this is either an auth service or AD
            var cookieId = ControllerContext.HttpContext.Request.Cookies["AMZ_WALLET_ID"];
            if (!string.IsNullOrWhiteSpace(cookieId))
            {
                return new GenericIdentity(cookieId);
            }

            cookieId = Guid.NewGuid().ToString(); 
            ControllerContext.HttpContext.Response.Cookies.Append("AMZ_WALLET_ID", cookieId);
            
            return new GenericIdentity(cookieId);
        }
    }
}