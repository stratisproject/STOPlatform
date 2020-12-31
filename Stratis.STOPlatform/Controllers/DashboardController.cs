using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Serilog;
using Stratis.STOPlatform.Core;
using Stratis.STOPlatform.Core.Extensions;
using Stratis.STOPlatform.Core.Providers;
using Stratis.STOPlatform.Core.Services;
using Stratis.STOPlatform.Core.Swagger;
using Stratis.STOPlatform.Data;
using Stratis.STOPlatform.Data.Docs;
using Stratis.STOPlatform.Data.Entities;
using Stratis.STOPlatform.Models.DashboardViewModels;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Stratis.STOPlatform.Controllers
{
    [Authorize]
    public class DashboardController : AppController
    {
        private readonly ApplicationDbContext database;
        private readonly AppSetting setting;
        private readonly ISTOService stoService;
        private readonly ICoinInfoProvider coinInfoProvider;
        private readonly IMemoryCache cache;
        private readonly ISwaggerClient swaggerClient;
        private readonly ILogger logger = Log.ForContext<DashboardController>();

        public DashboardController(
            ApplicationDbContext database,
            AppSetting setting,
            ISTOService stoService,
            ISwaggerClient swaggerClient,
            ICoinInfoProvider coinInfoProvider,
            IMemoryCache cache
            )
        {
            this.database = database;
            this.setting = setting;
            this.stoService = stoService;
            this.coinInfoProvider = coinInfoProvider;
            this.cache = cache;
            this.swaggerClient = swaggerClient;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> Index()
        {
            var deposits = await stoService.GetUpdatedDepositsAsync(UserId);

            var setting = await database.Documents.GetAsync<STOSetting>();

            var model = new DashboardIndexViewModel(deposits, setting, coinInfoProvider.Cirrus)
            {
                STOFinished = await cache.GetOrCreateAsync(CacheKeys.STOEnded, async entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
                    return await swaggerClient.IsSTOEndedAsync(setting.ContractAddress);
                })
            };

            if (setting.ShowTotalContribution)
            {
                model.TotalContribution = await cache.GetOrCreateAsync(CacheKeys.Balance, async entry =>
                {
                    entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
                    return await swaggerClient.GetBalanceAsync(setting.ContractAddress);
                });
            }

            return View(model);
        }
    }
}