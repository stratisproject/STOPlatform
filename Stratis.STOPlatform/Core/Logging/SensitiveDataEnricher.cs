using System.Linq;
using System.Text.RegularExpressions;
using Serilog.Core;
using Serilog.Events;

// ReSharper disable InlineOutVariableDeclaration

namespace Stratis.STOPlatform.Core.Logging
{
    public class SensitiveDataEnricher : ILogEventEnricher
    {
        private const string Mask = "******";
        private const string PasswordPattern = @"(sharedaccesskey|pwd|password)=([^;]*)[;=]";

        public SensitiveDataEnricher(params string[] propertyNames)
        {
            this.PropertyNames = propertyNames
                .Where(x => x != null)
                .Select(x => x.ToLowerInvariant().Trim())
                .Distinct()
                .ToArray();

            this.ScalarReplacement = new ScalarValue(Mask);
        }

        public static string[] DefaultPropertyNames => new[]
        {
            "ConnectionString",
            "ApiKey",
            "ApiSubscriptionKey",
            "Microsoft.ServiceBus.ConnectionString",
            "BearerToken",
            "Token"
        };

        private bool IsRegex(string name)
        {
            if (name.EndsWith("connectionstring"))
                return true;
            if (name.EndsWith("connection"))
                return true;
            return false;
        }

        public string[] PropertyNames { get; }

        public ScalarValue ScalarReplacement { get; set; }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var properties = logEvent.Properties.Keys;
            foreach (var name in properties)
            {
                if (string.IsNullOrEmpty(name))
                    continue;

                var key = name.ToLowerInvariant().Trim();
                if (!this.PropertyNames.Contains(key))
                    continue;

                if (this.IsRegex(key))
                {
                    var replacement = this.ScalarReplacement;
                    LogEventPropertyValue value;
                    if (!logEvent.Properties.TryGetValue(name, out value))
                        return;

                    try
                    {
                        var scalar = ((value as ScalarValue)?.Value as string);
                        if (scalar != null)
                        {
                            var matches = Regex.Match(scalar, PasswordPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
                            if (matches.Success)
                            {
                                foreach (Capture capture in matches.Captures)
                                {
                                    var password = capture.Value;
                                    scalar = scalar.Replace(password, "Password=" + Mask + ";");
                                }
                                replacement = new ScalarValue(scalar);
                            }
                        }
                        var property = new LogEventProperty(name, replacement);
                        logEvent.AddOrUpdateProperty(property);
                    }
                    catch
                    {
                        logEvent.AddPropertyIfAbsent(new LogEventProperty("ContainsSensitiveData", new ScalarValue(true)));
                    }
                }
                else
                {
                    var property = new LogEventProperty(name, this.ScalarReplacement);
                    logEvent.AddOrUpdateProperty(property);
                }
            }
        }
    }
}