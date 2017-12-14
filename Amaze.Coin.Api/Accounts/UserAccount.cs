using NBitcoin;
using Nethereum.HdWallet;

namespace Amaze.Coin.Stores.Accounts
{
    public class UserAccount
    {
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public int Balance { get; set; }
        public Wallet Wallet { get; set; }

        public static UserAccount Initialize(string userName, string displayName = null)
        {
            return new UserAccount
            {
                UserName = userName,
                DisplayName = displayName,
                Wallet = new Wallet(Wordlist.English, WordCount.Twelve),
                Balance = 0
            };
        }

    }
}
