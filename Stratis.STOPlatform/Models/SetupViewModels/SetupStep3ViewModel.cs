using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Stratis.STOPlatform.Core;
using Stratis.STOPlatform.Core.Extensions;
using Stratis.STOPlatform.Data.Docs;
using Stratis.STOPlatform.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Stratis.STOPlatform.Models.SetupViewModels
{
    public class SetupStep3ViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Contact Email")]
        public string ContactEmail { get; set; }

        [Required]
        [Url]
        [Display(Name = "Terms & Conditions Url")]
        public string TermsAndConditionsUrl { get; set; }

        [Compare("True", ErrorMessage = "Please agree to disclaimer conditions")]
        public bool DisclaimerAgreement { get; set; }

        public bool True { get; set; }

        public SetupStep3ViewModel()
        {

        }

        public SetupStep3ViewModel(STOSetting setting)
        {
            this.TermsAndConditionsUrl = setting.TermsAndConditionsUrl;
        }

        public void UpdateEntity(STOSetting setting)
        {
            setting.ContactEmail = ContactEmail;
            setting.TermsAndConditionsUrl = TermsAndConditionsUrl;
        }
    }
}