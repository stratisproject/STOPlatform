using System;
using System.Diagnostics;
using System.Reflection;
using Serilog.Core;
using Serilog.Events;

namespace Stratis.STOPlatform.Core.Logging
{
    public class SourceSystemInformationalVersionEnricher<T> : SourceSystemInformationalVersionEnricher
    {
        public SourceSystemInformationalVersionEnricher()
            : base(typeof(T).GetTypeInfo().Assembly)
        {
        }
    }

    public class SourceSystemInformationalVersionEnricher : ILogEventEnricher
    {
        private readonly string version;

        public SourceSystemInformationalVersionEnricher(Assembly assembly)
        {
            if (assembly == null)
                throw new ArgumentNullException(nameof(assembly));
            try
            {
                this.version = FileVersionInfo.GetVersionInfo(assembly.Location).ProductVersion;
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch (Exception)
            {
            }
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (string.IsNullOrWhiteSpace(this.version))
                return;
            logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty("PackageVersion", this.version, false));
        }
    }
}