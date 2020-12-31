namespace Stratis.STOPlatform.Core.Swagger
{
    public class Receipt
    {
        public string TransactionHash { get; set; }
        public string BlockHash { get; set; }
        public string PostState { get; set; }
        public ulong GasUsed { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string NewContractAddress { get; set; }
        public bool Success { get; set; }
        public bool? ReturnValue { get; set; }
        public string Bloom { get; set; }
        public string Error { get; set; }
        public LogDetails[] Logs { get; set; }


        public class LogDetails
        {
            public InvestLog Log { get; set; }
        }

        public class InvestLog
        {
            public string Address { get; set; }
            public ulong Invested { get; set; }
            public ulong TokenAmount { get; set; }
            public ulong Refunded { get; set; }
        }
    }
}

