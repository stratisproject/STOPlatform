using Microsoft.AspNetCore.Http;
using Stratis.STOPlatform.Controllers;
using Stratis.STOPlatform.Core;
using System;

namespace Microsoft.AspNetCore.Mvc
{
    public static class UrlHelperExtensions
    {
        public static string DashboardLink(this IUrlHelper urlHelper, string scheme = null)
        {
            return urlHelper.Action(
                action: nameof(DashboardController.Index),
                controller: Utility.ControllerName<DashboardController>(),
                values: new { },
                protocol: scheme);
        }

        /// <summary>
        /// Generates a fully qualified URL to the specified content by using the specified content path. Converts a
        /// virtual (relative) path to an application absolute path.
        /// </summary>
        /// <param name="url">The URL helper.</param>
        /// <param name="contentPath">The content path.</param>
        /// <returns>The absolute URL.</returns>
        public static string AbsoluteContent(
            this IUrlHelper url,
            string contentPath)
        {
            var request = url.ActionContext.HttpContext.Request;
            return new Uri(new Uri(request.Scheme + "://" + request.Host.Value), url.Content(contentPath)).ToString();
        }
    }
}
