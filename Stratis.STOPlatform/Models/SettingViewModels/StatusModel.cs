using Stratis.STOPlatform.Core.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stratis.STOPlatform.Models.SettingViewModels
{
    public class StatusModel
    {
        public decimal TotalContribution { get; set; }
        public CoinInfo CoinInfo { get; set; }
    }
}
