using Stratis.STOPlatform.Core.Providers;
using Stratis.STOPlatform.Data.Entities;

namespace Stratis.STOPlatform.Core.Services
{    public class TotalContribution
    {
        public CoinInfo CoinInfo { get; set; }
        public decimal Total { get; set; }
        public decimal TotalInUsd { get; set; }
        public bool UseUsdPrice { get; set; }
    }
}