using System.Collections.Generic;
using System.Threading.Tasks;
using Stratis.STOPlatform.Data.Entities;

namespace Stratis.STOPlatform.Core.Providers
{
    public interface ICoinInfoProvider
    {
        CoinInfo Cirrus { get; }
    }
}