using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using Stratis.STOPlatform.Core;
using Stratis.STOPlatform.Core.Providers;
using Stratis.STOPlatform.Data.Docs;
using Stratis.STOPlatform.Data.Entities;
using Stratis.SmartContracts.CLR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Stratis.STOPlatform.Models.SettingViewModels
{
    public class SettingViewModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Token Name")]
        public string TokenName { get; set; }

        public string Symbol { get; set; }

        [Display(Name = "STO Contract Address")]
        public string ContractAddress { get; set; }

        [Display(Name = "Token Contract Address")]
        public string TokenAddress { get; set;  }

        [Display(Name = "Owner Address")]
        public string OwnerAddress { get; set; }

        [Display(Name = "Total Supply")]
        public ulong TotalSupply { get; set; }

        [Required]
        [Url]
        [Display(Name = "Website Url")]
        public string WebsiteUrl { get; set; }

        [Required]
        [Url]
        [Display(Name = "Blog Post Url")]
        public string BlogPostUrl { get; set; }

        [Required]
        [Url]
        [Display(Name = "Terms And Conditions Url")]
        public string TermsAndConditionsUrl { get; set; }

        public bool ShowTotalContribution { get; set; }

        public SettingViewModel()
        {

        }

        public SettingViewModel(STOSetting setting)
        {
            Name = setting.Name;
            WebsiteUrl = setting.WebsiteUrl;
            BlogPostUrl = setting.BlogPostUrl;
            TermsAndConditionsUrl = setting.TermsAndConditionsUrl;
            ShowTotalContribution = setting.ShowTotalContribution;
            ///Readonly fields
            TokenName = setting.TokenName;
            Symbol = setting.Symbol;
            ContractAddress = setting.ContractAddress;
            TokenAddress = setting.TokenAddress;
            OwnerAddress = setting.Owner;
            TotalSupply = setting.TotalSupply;
        }

        public void UpdateEntity(STOSetting setting)
        {
            setting.Name = Name;
            setting.WebsiteUrl = WebsiteUrl;
            setting.BlogPostUrl = BlogPostUrl;
            setting.TermsAndConditionsUrl = TermsAndConditionsUrl;
            setting.ShowTotalContribution = ShowTotalContribution;
        }
    }
}