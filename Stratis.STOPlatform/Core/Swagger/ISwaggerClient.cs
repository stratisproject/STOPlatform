using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stratis.STOPlatform.Core.Swagger
{
    public interface ISwaggerClient
    {
        Task<string> DeploySmartContractAsync(DeployContractParameter parameters);
        Task<IEnumerable<Receipt>> GetReceiptsAsync(string contractAddress,string eventName,string[] topics);
        Task<IEnumerable<WalletAddress>> GetAddressesAsync(string walletName);
        Task<IEnumerable<string>> GetWalletsAsync();
        Task<bool> IsSTOEndedAsync(string contractAddress);
        Task<decimal> GetBalanceAsync(string contractAddress);
        Task<string> GetPrimaryAddressAsync(string contractAddress,string address);
    }
}