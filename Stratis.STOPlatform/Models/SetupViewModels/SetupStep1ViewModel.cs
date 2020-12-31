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
    public class SetupStep1ViewModel
    {

        [Compare("True", ErrorMessage = "Please agree to disclaimer conditions")]
        public bool DisclaimerAgreement { get; set; }

        public bool True { get; set; }

    }
}