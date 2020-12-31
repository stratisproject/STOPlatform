namespace Stratis.STOPlatform.Core.Swagger
{
    public class DeployContractApiParameters
    {
        public int Amount { get; set; }
        public string WalletName { get; set; }
        public string Sender { get; set; }
        public string Password { get; set; }
        public string[] Parameters { get; set; }
        public int GasPrice { get; set; }
        public int GasLimit { get; set; }
        public string ContractCode { get; set; }
        public decimal FeeAmount { get; set; }
    }
}