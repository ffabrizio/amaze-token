using NBitcoin;
using Nethereum.HdWallet;

namespace Amaze.Coin.Api.Models
{
    public class UserAccount
    {
        public string UserName { get; private set; }
        public string DisplayName { get; private set; }
        public Wallet Wallet { get; private set; }
        
        public static UserAccount Initialize(string userName, string displayName = null)
        {
            return new UserAccount
            {
                UserName = userName,
                DisplayName = displayName,
                Wallet = new Wallet(Wordlist.English, WordCount.Twelve)
            };
        }

    }
}
