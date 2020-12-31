namespace Stratis.STOPlatform.Core.Swagger
{
    public class WalletAddressResponse
    {
        public WalletAddress[] Addresses { get; set; }
    }
    public class WalletAddress
    {
        public string Address { get; set; }
        public bool IsChange { get; set; }
        public long AmountConfirmed { get; set; }
    }

}