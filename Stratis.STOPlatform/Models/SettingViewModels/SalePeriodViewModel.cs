using System;

namespace Stratis.STOPlatform.Models.SettingViewModels
{
        public class SalePeriodViewModel
        {
            public DateTimeOffset EndDate { get; set; }

            public decimal? StratAmount { get; set; }

            public decimal? BtcAmount { get; set; }

            public decimal? UsdPrice { get; set; }
    }
        
}