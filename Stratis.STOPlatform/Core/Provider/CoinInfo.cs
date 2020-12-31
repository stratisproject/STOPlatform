using NBitcoin;
using Stratis.STOPlatform.Data.Entities;
using System;

namespace Stratis.STOPlatform.Core.Providers
{
    public class CoinInfo
    {
        public string TokenName { get; set; }
        public string FullName { get; set; }
        public Func<string, string> TransactionLink { get; set; }
        public Func<string, string> AddressExploreLink { get; set; }
        public Network Network { get; internal set; }
    }
}
