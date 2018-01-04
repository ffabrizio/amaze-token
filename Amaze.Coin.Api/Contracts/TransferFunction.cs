using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts.CQS;

namespace Amaze.Coin.Api.Contracts
{
    [Function("transfer", "bool")]
    public class TransferFunction : ContractMessage
    {
        [Parameter("address", "_to")]
        public string To { get; set; }

        [Parameter("uint256", "_value", 2)]
        public int TokenAmount { get; set; }
    }
}