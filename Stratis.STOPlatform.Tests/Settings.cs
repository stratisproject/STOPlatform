using Microsoft.Extensions.Configuration;
using Stratis.STOPlatform.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stratis.STOPlatform.Tests
{
    public class Settings
    {
        public static AppSetting Config { get; }

        static Settings()
        {
            Config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build()
                .GetSection("AppSettings")
                .Get<AppSetting>();
        }
    }
}
