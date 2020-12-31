using System;

namespace Stratis.STOPlatform.Core
{
    public class AppSetting
    {
        public Uri SwaggerUrl { get; set; }

        public string KYCContractAddress { get; set; }
        public string MapperContractAddress { get; set; }

        public bool TestNet { get; set; }
    }
}