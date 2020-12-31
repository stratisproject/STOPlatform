using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using Stratis.STOPlatform.Core.Swagger;
using Stratis.STOPlatform.Data;
using Stratis.STOPlatform.Data.Docs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stratis.STOPlatform.Components
{
    public class Navigation : ViewComponent
    {
        private readonly ApplicationDbContext database;
        private readonly ISwaggerClient client;

        public Navigation(ApplicationDbContext database, ISwaggerClient client)
        {
            this.database = database;
            this.client = client;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var setting = await database.Documents.GetAsync<STOSetting>();
            var stoFinished = await client.IsSTOEndedAsync(setting.ContractAddress);
            return View((setting,stoFinished));
        }
    }
}
