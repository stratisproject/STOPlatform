using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace Stratis.STOPlatform.Core.Filters
{
    public class WebApiRequestLogFilter : IAsyncActionFilter
    {
        private readonly ILogger logger;

        private readonly Stopwatch stopWatch;

        public WebApiRequestLogFilter(ILogger logger)
        {
            this.logger = logger;
            this.stopWatch = Stopwatch.StartNew();
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext actionContext, ActionExecutionDelegate next)
        {
            var currentLogger = this.logger;
            var request = actionContext.HttpContext.Request;
            var requestHeaders = request.Headers;
            foreach (var kvp in requestHeaders.AsQueryable())
            {
                if (kvp.Key == "Authorization") continue;
                currentLogger = currentLogger.ForContext(kvp.Key, kvp.Value);
            }

            var httpMethod = request.Method;
            var requestUrl = request.GetDisplayUrl();

            this.logger.Debug("HTTP {HttpMethod} to {RawUrl} ({@RequestHeaders}) {RequestState}",
                httpMethod,
                requestUrl,
                requestHeaders,
                "started");

            var actionExecutedContext = await next();

            this.stopWatch.Stop();

            var exception = actionExecutedContext.Exception;

            if (exception == null)
            {
                this.logger.Information("HTTP {HttpMethod} ({RequestDuration}) to {RawUrl} {RequestState}",
                    httpMethod,
                    this.stopWatch.Elapsed,
                    requestUrl,
                    "completed");
            }
            else
            {
                var logContext = this.logger;
                foreach (var key in exception.Data.Keys.OfType<string>())
                {
                    logContext = logContext.ForContext(key, exception.Data[key]);
                }

                logContext.Error(exception, "HTTP {HttpMethod} ({RequestDuration}) to {RawUrl} {RequestState} ({Message})",
                    httpMethod,
                    this.stopWatch.Elapsed,
                    requestUrl,
                    "failed",
                    exception.Message);
            }
        }
    }
}
