using DBreeze.Utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;
using NBitcoin;
using Newtonsoft.Json;
using Refit;
using Serilog;
using Stratis.STOPlatform.Core;
using Stratis.STOPlatform.Core.Providers;
using Stratis.STOPlatform.Core.Services;
using Stratis.STOPlatform.Core.Swagger;
using Stratis.STOPlatform.Data;
using Stratis.STOPlatform.Data.Docs;
using Stratis.STOPlatform.Models.SetupViewModels;
using Stratis.SmartContracts;
using System;
using System.Linq;
using System.Threading.Tasks;
using ILogger = Serilog.ILogger;

namespace Stratis.STOPlatform.Controllers
{
    [Route("[controller]")]
    public class SetupController : AppController
    {
        private readonly ApplicationDbContext database;
        private readonly ISwaggerClient swaggerClient;
        private readonly ICoinInfoProvider coinInfoProvider;
        private readonly ILogger logger = Log.ForContext<SettingController>();

        public SetupController(ApplicationDbContext database, ISwaggerClient swaggerClient, ICoinInfoProvider coinInfoProvider)
        {
            this.database = database;
            this.swaggerClient = swaggerClient;
            this.coinInfoProvider = coinInfoProvider;
        }

        [HttpGet("step-1")]
        public async Task<IActionResult> Step1()
        {
            var setup = await database.Documents.GetAsync<SetupSetting>();

            if (setup.Done)
                return Redirect("/");

            return View();
        }

        [HttpPost("step-1")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Step1(ModelStateDictionary modelState)
        {
            
            var setup = await database.Documents.GetAsync<SetupSetting>();

            if (setup.Done)
                return Redirect("/");

            setup.CurrentStep = 2;

            await database.SaveChangesAsync();

            return Redirect("/");
        }


        [HttpGet("step-2")]
        public async Task<IActionResult> Step2()
        {
            var setup = await database.Documents.GetAsync<SetupSetting>();

            if (setup.Done)
                return Redirect("/");

            var model = new SetupStep2ViewModel();

            return await Step2ViewAsync(model);
        }

        public async Task<IActionResult> Step2ViewAsync(SetupStep2ViewModel model)
        {
            var wallets = await swaggerClient.GetWalletsAsync();
            model.Wallets = wallets.Select(w => new SelectListItem(w, w));

            foreach (var wallet in wallets)
            {
                var addresses = await swaggerClient.GetAddressesAsync(wallet);
                var result = addresses.OrderByDescending(a => a.AmountConfirmed)
                                     .Take(20)
                                     .Select(a => new SetupStep2ViewModel.WalletAddress { Wallet = wallet, Address = a.Address, Amount = Money.Satoshis(a.AmountConfirmed).ToDecimal(MoneyUnit.BTC) });

                model.Addresses =  model.Addresses.Concat(result);

            }
            return View(model);
        }

        [HttpPost("step-2")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Step2(SetupStep2ViewModel model)
        {
            if (!ModelState.IsValid || !await ValidateAddressAsync(model))
            {
                return await Step2ViewAsync(model);
            }

            try
            {
                var setting = await database.Documents.GetAsync<STOSetting>();
                var setup = await database.Documents.GetAsync<SetupSetting>();

                if (setup.Done)
                    return Redirect("/");

                var parameters = model.ToContractParameter();
                var transactionId = await swaggerClient.DeploySmartContractAsync(parameters);
                model.UpdateSetting(setting, transactionId, coinInfoProvider.Cirrus.Network);
                setup.CurrentStep = 3;

                await database.SaveChangesAsync();

                AlertMessage(AlertType.Success, $"The Contract have been submited successfully! The transaction is waiting to be confirmed.");
            }
            catch (ApiException ex)
            {
                var response = JsonConvert.DeserializeObject<SwaggerErrorResponse>(ex.Content);
                ModelState.AddModelError(string.Empty, response.errors[0].Message);
                return await Step2ViewAsync(model);
            }

            
            return Redirect("/");
        }

        private async Task<bool> ValidateAddressAsync(SetupStep2ViewModel model)
        {
            var result = await swaggerClient.GetAddressesAsync(model.WalletName);

            if (result.Any(r => r.Address == model.Owner))
            {
                ModelState.AddModelError(nameof(model.Owner), "Owner address should not be from current wallet.");
                return false;
            }

            return true;
        }

        [HttpGet("ValidateOwner")]
        public ActionResult ValidateOwner(string owner)
        {
            var valid = Utility.ValidateAddress(owner, coinInfoProvider.Cirrus.Network);
            return Json(valid);
        }

        [HttpGet("step-3")]
        public async Task<IActionResult> Step3()
        {
            var setup = await database.Documents.GetAsync<SetupSetting>();

            if (setup.Done)
                return Redirect("/");

            var setting = await database.Documents.GetAsync<STOSetting>();
            var model = new SetupStep3ViewModel(setting);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HttpPost("step-3")]
        public async Task<IActionResult> Step3(SetupStep3ViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var setting = await database.Documents.GetAsync<STOSetting>();

            model.UpdateEntity(setting);

            var setup = await database.Documents.GetAsync<SetupSetting>();
            setup.Done = true;
            await database.SaveChangesAsync();

            AlertMessage(AlertType.Success, "Welcome to Stratis STO Platform.", "Set up is done!");

            await HttpContext.SignOutAsync("cookie");
            
            return Redirect("/");
        }
    }
}