using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Stratis.STOPlatform.Core;
using Stratis.STOPlatform.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Stratis.STOPlatform.Core
{
    public static class HtmlHelpers
    {
        public static string IsSelected(this IHtmlHelper html, string controller, string action)
        { 
            var currentAction = (string)html.ViewContext.RouteData.Values["action"];
            var currentController = (string)html.ViewContext.RouteData.Values["controller"];

            var selected = controller.Equals(currentController, StringComparison.InvariantCultureIgnoreCase)
                    && action.Equals(currentAction, StringComparison.InvariantCultureIgnoreCase);

            return  selected ? "active" : String.Empty;
        }

        public static IHtmlContent Json(this IHtmlHelper html, object obj)
        {
            return html.Raw(JsonConvert.SerializeObject(obj));
        }
    }
}
