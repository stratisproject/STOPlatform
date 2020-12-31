using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Refit;
using Stratis.STOPlatform.Core;
using Stratis.STOPlatform.Core.Swagger;

namespace Stratis.STOPlatform.Tests
{
    [TestClass]
    public class SwaggerClientTests
    {
        private const string Address = "CQZrkndqi6Ab9tftqx5MGSGPZydVHi5bMk";

        [Ignore]
        public async Task GetTransactions()
        {
            var baseUrl = Settings.Config.SwaggerUrl.ToString();
            var client = RestService.For<ISwaggerApi>(baseUrl);
            var results = await client.GetReceiptsAsync(new ReceiptSearchParameters { ContractAddress = Address });

            Assert.IsTrue(results.Any());
        }
    }
}