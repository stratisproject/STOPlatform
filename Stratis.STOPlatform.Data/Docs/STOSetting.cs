using Newtonsoft.Json;
using Stratis.STOPlatform.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Stratis.STOPlatform.Data.Docs
{
    public class STOSetting
    {
        public string Name { get; set; }
        public string TokenName { get; set; }
        public string Symbol { get; set; }
        public string ContractAddress { get; set; }
        public string TokenAddress { get; set; }
        public ulong TotalSupply { get; set; }
        public string Owner { get; set; }
        public string ContactEmail { get; set; }
        public bool ShowTotalContribution { get; set; }

        #region Content
        public string PageCover { get; set; }
        public string WebsiteUrl { get; set; }
        public string Slogan { get; set; }
        public string BlogPostUrl { get; set; }
        public string TermsAndConditionsUrl { get; set; }
        public string LogoSrc { get; set; }
        public string BackgroundSrc { get; set; }
        public string LoginBackgroundSrc { get; set; }
        #endregion
    }
}