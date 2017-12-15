namespace Amaze.Coin.Api
{
    public class AppSettings
    {
        public string RpcEndpoint { get; set; }
        public string AdminSeed { get; set; }
        public string AdminPwd { get; set; }
        public int TokensOnAccountCreation { get; set; }
        public string CoinContractAddress { get; set; }
    }
}
