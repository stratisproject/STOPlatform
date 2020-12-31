using CsvHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Stratis.STOPlatform.Core;
using Stratis.STOPlatform.Core.CsvMappings;
using Stratis.STOPlatform.Core.Providers;
using Stratis.STOPlatform.Core.Services;
using Stratis.STOPlatform.Core.Swagger;
using Stratis.STOPlatform.Data;
using Stratis.STOPlatform.Data.Docs;
using Stratis.STOPlatform.Data.Entities;
using Stratis.STOPlatform.Models.SettingViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ILogger = Serilog.ILogger;

namespace Stratis.STOPlatform.Controllers
{
    [Authorize(Roles = AppConstants.AdminRole)]
    public class SettingController : AppController
    {
        private readonly AppSetting setting;
        private readonly ApplicationDbContext database;
        private readonly IWebHostEnvironment environment;
        private readonly ISTOService stoService;
        private readonly ISwaggerClient swaggerClient;
        private readonly ICoinInfoProvider coinInfoProvider;
        private readonly ILogger logger = Log.ForContext<SettingController>();

        public SettingController(
            ApplicationDbContext database,
            AppSetting setting,
            IWebHostEnvironment environment,
            ISTOService stoService,
            ISwaggerClient swaggerClient,
            ICoinInfoProvider coinInfoProvider
            )
        {
            this.database = database;
            this.setting = setting;
            this.environment = environment;
            this.stoService = stoService;
            this.swaggerClient = swaggerClient;
            this.coinInfoProvider = coinInfoProvider;
        }

        public async Task<IActionResult> Edit()
        {
            var setting = await database.Documents.GetAsync<STOSetting>();

            var model = new SettingViewModel(setting);

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SettingViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var setting = await database.Documents.GetAsync<STOSetting>();

            model.UpdateEntity(setting);

            await database.SaveChangesAsync();

            AlertMessage(AlertType.Success, "Your changes have been saved successfully!");

            return RedirectToAction(nameof(Edit));
        }


        public async Task<IActionResult> ExportContributions()
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            using (var csv = new CsvWriter(writer, cultureInfo: CultureInfo.CurrentCulture))
            {
                csv.Configuration.RegisterClassMap<UserDepositMap>();
                var deposits = await GetDepositsAsync();
                csv.WriteRecords(deposits);
                await writer.FlushAsync();
                stream.Seek(0, SeekOrigin.Begin);
                return File(stream, "text/csv", fileDownloadName: "contributions.csv");
            }
        }

        async Task<IEnumerable<UserDepositCsv>> GetDepositsAsync()
        {
            var setting = await database.Documents.GetAsync<STOSetting>();
            return database.Deposits
                .OrderBy(u => u.User.Id)
                .ThenBy(u => u.Date)
                .Select(u => new
                {
                    u.User.FullName,
                    u.Invested,
                    u.EarnedToken,
                    u.Date
                })
                .AsEnumerable()
                .Select(u =>
                {
                    var format = "G9";
                    var amount = u.Invested.ToString(format) + " " + coinInfoProvider.Cirrus.TokenName;
                    return new UserDepositCsv
                    {
                        FullName = u.FullName,
                        Invested = amount,
                        EarnedToken = $"{u.EarnedToken.ToString(format)} {setting.Symbol.ToUpperInvariant()}",
                        Date = u.Date
                    };
                });
        }

        public async Task<IActionResult> Status()
        {
            var setting = await database.Documents.GetAsync<STOSetting>();
            var model = new StatusModel
            {
                TotalContribution = await swaggerClient.GetBalanceAsync(setting.ContractAddress),
                CoinInfo = coinInfoProvider.Cirrus
            };

            return View(model);
        }

        public async Task<IActionResult> Content()
        {
            var setting = await database.Documents.GetAsync<STOSetting>();

            var path = Path.Combine(environment.WebRootPath, AppConstants.DefaultCssStylePath);

            var model = new ContentViewModel(setting)
            {
                CssStyle = System.IO.File.ReadAllText(path),
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Content(ContentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var setting = await database.Documents.GetAsync<STOSetting>();

            model.UpdateEntity(setting);

            var previousImages = new List<string>();

            if (model.LogoImage != null)
            {
                previousImages.Add(setting.LogoSrc);
                setting.LogoSrc = await SaveImageAsync(model.LogoImage);
            }

            if (model.BackgroundImage != null)
            {
                previousImages.Add(setting.BackgroundSrc);
                setting.BackgroundSrc = await SaveImageAsync(model.BackgroundImage);
            }

            if (model.LoginBackgroundImage != null)
            {
                previousImages.Add(setting.LoginBackgroundSrc);
                setting.LoginBackgroundSrc = await SaveImageAsync(model.LoginBackgroundImage);
            }

            await SaveCssStyleAsync(model.CssStyle);

            await database.SaveChangesAsync();

            foreach (var path in previousImages)
            {
                DeleteImage(path);
            }

            AlertMessage(AlertType.Success, "Your changes have been saved successfully!");

            return RedirectToAction(nameof(Content));

            void DeleteImage(string filePath)
            {
                var file = environment.WebRootFileProvider.GetFileInfo(filePath);
                var inUploadFolder = Path.GetDirectoryName(filePath) == $@"\{AppConstants.UploadFolder}";
                if (inUploadFolder && file.Exists)
                    System.IO.File.Delete(file.PhysicalPath);
            }
        }

        async Task SaveCssStyleAsync(string cssStyle)
        {
            var path = Path.Combine(environment.WebRootPath, AppConstants.DefaultCssStylePath);
            await System.IO.File.WriteAllTextAsync(path, cssStyle);
        }

        async Task<string> SaveImageAsync(IFormFile image)
        {
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(image.FileName)}";

            var physicalPath = Path.Combine(environment.WebRootPath, AppConstants.UploadFolder, fileName);

            using (var stream = new FileStream(physicalPath, FileMode.Create))
            {
                await image.CopyToAsync(stream);

                return $"/{AppConstants.UploadFolder}/{fileName}";
            }
        }
    }
}