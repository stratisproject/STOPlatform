using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Stratis.STOPlatform.Core.Extensions
{
    public static class HttpClientExtensions
    {
        public async static Task<HttpResponseMessage> PostAsJsonAsync(this HttpClient client, string url, object data)
        {
            return await client.PostAsync(url, GetJsonContent(data));
        }

        private static StringContent GetJsonContent(object data)
        {
            var json = JsonConvert.SerializeObject(data);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}
