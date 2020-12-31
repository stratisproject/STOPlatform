using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using SerilogWeb.Classic.Enrichers;
using Stratis.STOPlatform.Core;
using Stratis.STOPlatform.Core.Logging;
using System;
using System.Collections.Generic;
using System.IO;

namespace Stratis.STOPlatform
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            var logConfigurations = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .Enrich.With<HttpRequestIdEnricher>()
                .Enrich.With<UserNameEnricher>()
                .Enrich.With(new ExceptionEnricher())
                .Enrich.With<SourceSystemInformationalVersionEnricher<Program>>()
                .WriteTo.ColoredConsole()
                .WriteTo.RollingFile("/logs/log-{Date}.txt")
                .WriteTo.Trace(); // <-- this should give us log stream in azure
            if (!string.IsNullOrEmpty(configuration.GetConnectionString("LogStorage")))
            {
                logConfigurations
                    .WriteTo.File(@"D:\home\LogFiles\Application\stowebsite.txt",
                        fileSizeLimitBytes: 1_000_000,
                        rollOnFileSizeLimit: true,
                        shared: true,
                        flushToDiskInterval: TimeSpan.FromSeconds(1))
                    .WriteTo.AzureTableStorage(configuration.GetConnectionString("LogStorage"));
            }

            Log.Logger = logConfigurations.CreateLogger();
            try
            {
                Log.Information("Starting web host");
                BuildWebHost(args).Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var testnet = Convert.ToBoolean(Environment.GetEnvironmentVariable("STOPlatform_TestNet"));
                    var suffix = testnet ? ".TestNet" : "";
                    config.AddJsonFile($"networksettings{suffix}.json", optional: false, reloadOnChange: false);
                    config.AddInMemoryCollection(new[] { new KeyValuePair<string, string>("TestNet", testnet.ToString()) });
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddDebug();
                    logging.AddAzureWebAppDiagnostics();
                    logging.AddSerilog(Log.Logger);
                })
                .UseStartup<Startup>()
                .UseSerilog()
                .Build();
    }
}
