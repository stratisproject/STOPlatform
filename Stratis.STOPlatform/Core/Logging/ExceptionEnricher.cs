using System.Linq;
using Serilog.Core;
using Serilog.Events;

namespace Stratis.STOPlatform.Core.Logging
{
    public class ExceptionEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (logEvent.Exception == null)
                return;

            var ex = logEvent.Exception;
            var exceptionType = ex.GetType();
            var exceptionNamespace = exceptionType.Namespace ?? "";
            var exceptionMessageLine = (ex.Message ?? "").Split('\n').First();

            logEvent.AddPropertyIfAbsent(new LogEventProperty("ExceptionNamespace", new ScalarValue(exceptionNamespace)));
            logEvent.AddPropertyIfAbsent(new LogEventProperty("ExceptionType", new ScalarValue(exceptionType.FullName)));
            logEvent.AddPropertyIfAbsent(new LogEventProperty("ExceptionMessage", new ScalarValue(exceptionMessageLine)));
        }
    }
}
