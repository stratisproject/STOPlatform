namespace Stratis.STOPlatform.Core.Swagger
{
    public class LocalCallParameters
    {
        public string ContractAddress { get; set; }
        public string MethodName { get; set; }
        public int Amount { get; set; }
        public int GasPrice { get; set; }
        public int GasLimit { get; set; }
        public string Sender { get; set; }
        public string[] Parameters { get; set; }

        public LocalCallParameters()
        {
            GasPrice = 100;
            GasLimit = 100_000;
        }
    }
}