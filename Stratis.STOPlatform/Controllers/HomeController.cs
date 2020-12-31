using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Permissions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Stratis.STOPlatform.Data;
using Stratis.STOPlatform.Data.Docs;
using Stratis.STOPlatform.Models;

namespace Stratis.STOPlatform.Controllers
{
    public class HomeController : AppController
    {
        private readonly ILogger logger = Log.ForContext<HomeController>();
        private readonly ApplicationDbContext database;

        public HomeController(ApplicationDbContext database)
        {
            this.database = database;
        }

        [HttpGet("error")]
        public async Task<IActionResult> Error()
        {
            var contactEmail = string.Empty;
            try
            {
                var setting = await database.Documents.GetAsync<STOSetting>();
                contactEmail = setting?.ContactEmail;
            }
            catch (Exception e)
            {
                logger.Error(e, "Database call failed");
            }

            var requestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
            return View(new ErrorViewModel
            {
                ContactEmailLink = $"mailto:{contactEmail}?subject=Unexpected Error&body=@Request ID:{requestId}"
            });
        }
    }
}
