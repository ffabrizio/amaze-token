using System.Numerics;
using Nethereum.HdWallet;

namespace Amaze.Coin.Models
{
    public class AppVm
    {
        public bool IsNewAccount { get; set; }
        public Wallet Wallet { get; set; }
        public BigInteger Balance { get; set; }
    }
}
