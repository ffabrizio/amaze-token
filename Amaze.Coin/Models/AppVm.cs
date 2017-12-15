using Nethereum.HdWallet;
using System.Numerics;

namespace Amaze.Coin.Models
{
    public class AppVm
    {
        public bool IsNewAccount { get; set; }
        public Wallet Wallet { get; set; }
        public BigInteger Balance { get; set; }
    }
}
