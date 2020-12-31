using System;
using System.Collections.Generic;
using System.Text;

namespace Stratis.STOPlatform.Data.Docs
{
    public class GeoLocationSetting
    {
        public string GeoLocationApiKey { get; set; }
        public bool ProxyDisallowed { get; set; }

        public string[] BannedCountries { get; set; } = Array.Empty<string>();
    }
}
