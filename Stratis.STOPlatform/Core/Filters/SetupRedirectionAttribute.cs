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
    public class SetupRedirectionAttribute : IAsyncResultFilter
    {
        private readonly ApplicationDbContext database;

        private static bool setupDone;


        public SetupRedirectionAttribute(ApplicationDbContext database)
        {
            this.database = database;
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var (done, currentStep) = await GetSetupAsync();

            if (!done)
            {

                var request = context.HttpContext.Request;

                var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
                var actionName = "Step" + currentStep;
                var isCurrentStep = descriptor.ControllerTypeInfo == typeof(SetupController) && descriptor.ActionName == actionName;
                var isException = descriptor.ControllerTypeInfo == typeof(HomeController) && descriptor.ActionName == nameof(HomeController.Error);
                if (!request.Headers.IsAjaxRequest() && !isCurrentStep && !isException)
                {
                    context.Result = new RedirectToActionResult(actionName, Utility.ControllerName<SetupController>(), new { });
                }
            }

            await next();
        }

        private async Task<(bool done, int currentStep)> GetSetupAsync()
        {
            if (setupDone)
                return (done: true, currentStep: 0);

            var setup = await database.Documents.GetAsync<SetupSetting>();

            setupDone = setup.Done;
            return (done: setupDone, currentStep: setup.CurrentStep);
        }
    }
}
