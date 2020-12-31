using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Serilog;
using Stratis.STOPlatform.Core;
using Stratis.STOPlatform.Core.Extensions;
using Stratis.STOPlatform.Models;
using System.Net;
using System.Security.Claims;

namespace Stratis.STOPlatform.Controllers
{
    public abstract class AppController : Controller
    {
        public void AlertMessage(AlertType type, string message, string title = null)
        {
            TempData[AppConstants.AlertMessageKey] = JsonConvert.SerializeObject(new AlertMessage { Type = type, Message = message, Title = title });
        }

        protected string ControllerName<TController>() where TController : Controller => Utility.ControllerName<TController>();

        protected int UserId => User.FindFirst(ClaimTypes.NameIdentifier).Value.As<int>();
    }
}
