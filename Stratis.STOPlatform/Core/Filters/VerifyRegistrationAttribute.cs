using IdentityModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Stratis.STOPlatform.Controllers;
using Stratis.STOPlatform.Core.Extensions;
using Stratis.STOPlatform.Data;
using Stratis.STOPlatform.Data.Docs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stratis.STOPlatform.Core.Filters
{
    public class VerifyRegistrationAttribute : IAsyncActionFilter
    {
        private readonly AppSetting setting;

        public VerifyRegistrationAttribute(AppSetting setting)
        {
            this.setting = setting;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var user = context.HttpContext.User;
            if (user.Identity.IsAuthenticated)
            {
                if (!user.HasClaim("kyc", "Identity Approved"))
                {
                    var controller = context.Controller as AppController;
                    controller.AlertMessage(AlertType.Danger, "Your KYC is not verified!");

                    context.Result = controller.SignOut("cookie", "oidc");
                    return;
                }
                else if (!user.HasClaim(c => c.Type == "PrimaryAddress"))
                {
                    var controller = context.Controller as AppController;
                    var identityAddress = controller.User.FindFirst(JwtClaimTypes.Subject).Value;
                    
                    controller.AlertMessage(AlertType.Danger, $"You should associate your identity address to your wallet address to login! Make a call to MapAddress method of the contract {setting.MapperContractAddress} with address parameter {identityAddress} and approve it from your Identity mobile app.");

                    context.Result = controller.SignOut("cookie", "oidc");
                    return;
                }
            }
            await next();
        }
    }
}
