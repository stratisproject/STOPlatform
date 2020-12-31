using DBreeze.Utils;
using Microsoft.Net.Http.Headers;
using NBitcoin;
using Newtonsoft.Json;
using Stratis.STOPlatform.Core.Providers;
using Stratis.SmartContracts;
using Stratis.SmartContracts.CLR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stratis.STOPlatform.Core.Swagger
{
    public partial class SwaggerClient : ISwaggerClient
    {
        private readonly ISwaggerApi api;
        private readonly ISerializer serializer;
        private readonly ICoinInfoProvider coinInfoProvider;
        private readonly AppSetting appSetting;

        public SwaggerClient(ISwaggerApi api, ISerializer serializer, AppSetting appSetting, ICoinInfoProvider coinInfoProvider)
        {
            this.api = api;
            this.serializer = serializer;
            this.appSetting = appSetting;
            this.coinInfoProvider = coinInfoProvider;
        }

        public async Task<string> DeploySmartContractAsync(DeployContractParameter parameters)
        {
            var apiParameters = parameters.ToApiParameters(serializer, appSetting.KYCContractAddress, appSetting.MapperContractAddress);

            var id = await api.DeploySmartContractAsync(apiParameters);

            return JsonConvert.DeserializeObject<string>(id);
        }

        public async Task<IEnumerable<WalletAddress>> GetAddressesAsync(string walletName)
        {
            var result = await api.GetAddressesAsync(walletName);
            return result.Addresses;
        }

        public async Task<IEnumerable<Receipt>> GetReceiptsAsync(string contractAddress, string eventName, string[] topics)
        {
            return await api.GetReceiptsAsync(new ReceiptSearchParameters { ContractAddress = contractAddress, EventName = eventName, Topics = topics });
        }

        public async Task<IEnumerable<string>> GetWalletsAsync()
        {
            var response = await api.GetWalletsAsync();

            return response.WalletNames;
        }

        public async Task<bool> IsSTOEndedAsync(string contractAddress)
        {
            var parameter = new LocalCallParameters
            {
                ContractAddress = contractAddress,
                Sender = contractAddress,
                MethodName = "SaleOpen"
            };
            var response = await api.LocalCallAsync(parameter);
            return !(bool)response.Return;
        }

        public async Task<string> GetPrimaryAddressAsync(string contractAddress, string address)
        {
            if (!await IsMappingApprovedAsync(contractAddress, address))
                return null;

            var network = coinInfoProvider.Cirrus.Network;

            var parameter = new LocalCallParameters
            {
                ContractAddress = contractAddress,
                Sender = contractAddress,
                MethodName = "GetPrimaryAddress",
                Parameters = new[] { $"9#{address}" }
            };

            var response = await api.LocalCallAsync(parameter);
            var result = response.Return?.ToString();

            return result.ToAddress(network) == Address.Zero ? null : result;
        }

        private async Task<bool> IsMappingApprovedAsync(string contractAddress, string address)
        {
            var network = coinInfoProvider.Cirrus.Network;

            var parameter = new LocalCallParameters
            {
                ContractAddress = contractAddress,
                Sender = contractAddress,
                MethodName = "GetStatus",
                Parameters = new[] { $"9#{address}" }
            };

            var response = await api.LocalCallAsync(parameter);
            return int.TryParse(response.Return.ToString(), out var status) && status == 2;
        }

        public async Task<decimal> GetBalanceAsync(string contractAddress) => await api.GetBalanceAsync(contractAddress);

    }
}
