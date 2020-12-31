using Microsoft.AspNetCore.Mvc;
using Stratis.STOPlatform.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Stratis.STOPlatform.Models.DashboardViewModels
{
    public class WithdrawalSubmitViewModel
    {
        [Required]
        [Display(Name = "Token Address")]
        [Remote("ValidateAddress","Dashboard", ErrorMessage = "The {0} is invalid.")]
        public string Address { get; set; }

        [Display(Name = "Terms and Conditions")]
        public bool AgreeToTerms { get; set; }

        public string TermsUrl { get; set; }
    }
}
