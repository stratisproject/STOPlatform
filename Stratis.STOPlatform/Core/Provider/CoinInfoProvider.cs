using Stratis.STOPlatform.Data;
using Stratis.STOPlatform.Data.Docs;
using Stratis.STOPlatform.Data.Entities;
using Stratis.Sidechains.Networks;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stratis.STOPlatform.Core.Providers
{
    public class CoinInfoProvider : ICoinInfoProvider
    {
        public CoinInfo Cirrus { get; }

        public CoinInfoProvider(AppSetting appSetting)
        {
            var testnetPrefix = appSetting.TestNet ? "T" : "";
            var urlPrefix = appSetting.TestNet ? "test-" : "";
            var url = $"https://{urlPrefix}cirrusexplorer.stratisplatform.com/";
            var selector = CirrusNetwork.NetworksSelector;
            Cirrus = new CoinInfo
            {
                TokenName = testnetPrefix + "CRS",
                FullName = "Cirrus",
                TransactionLink = transactionId => $"{url}transactions/{transactionId}",
                AddressExploreLink = address => $"{url}addresses/{address}",
                Network = appSetting.TestNet ? selector.Testnet() : selector.Mainnet()
            };
        }
    }
}
