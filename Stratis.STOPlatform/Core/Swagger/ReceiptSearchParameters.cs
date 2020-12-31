namespace Stratis.STOPlatform.Core.Swagger
{
    public class ReceiptSearchParameters
    {
        public string ContractAddress { get; set; }
        public string EventName { get; set; }
        public string[] Topics { get; set; }
    }
}