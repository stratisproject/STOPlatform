using ICSharpCode.Decompiler.CSharp.Syntax;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stratis.STOPlatform.Core;
using Stratis.STOPlatform.Core.Providers;
using Stratis.STOPlatform.Data;
using Stratis.STOPlatform.Data.Docs;
using Stratis.STOPlatform.Data.Entities;
using Stratis.STOPlatform.Models.UserViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;
using X.PagedList;

namespace Stratis.STOPlatform.Controllers
{
    [Authorize(Roles = AppConstants.AdminRole)]
    public class UserController : AppController
    {
        private readonly ApplicationDbContext database;
        private readonly ICoinInfoProvider coinInfoProvider;

        public UserController(ApplicationDbContext database, ICoinInfoProvider coinInfoProvider)
        {
            this.database = database;
            this.coinInfoProvider = coinInfoProvider;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            var setting = await database.Documents.GetAsync<STOSetting>();

            var result = await database.Users.Select(u => new UserViewModel
                                        {
                                            Id = u.Id,
                                            FullName = u.FullName,
                                        }).ToPagedListAsync(page, 10);

            return View(result);
        }
    }
}