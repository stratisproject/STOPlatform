using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stratis.STOPlatform.Core;
using Stratis.STOPlatform.Data;
using Stratis.STOPlatform.Data.Docs;
using System.Threading.Tasks;

namespace Stratis.STOPlatform.Controllers
{
    [Authorize]
    public class AccountController : AppController
    {
        private readonly ApplicationDbContext database;

        public AccountController(ApplicationDbContext database)
        {
            this.database = database;
        }

        [HttpGet("", Order = 0)]
        [HttpGet("login", Order = 1)]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = null)
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToLocal(returnUrl);

            var stoSetting = await database.Documents.GetAsync<STOSetting>();
            return View(stoSetting);
        }

        [HttpPost("logout")]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            return SignOut("cookie", "oidc");
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(DashboardController.Index), ControllerName<DashboardController>());
            }
        }

    }
}
