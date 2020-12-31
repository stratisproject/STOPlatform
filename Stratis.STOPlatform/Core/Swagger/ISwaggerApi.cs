using Refit;
using Stratis.Bitcoin.Features.SmartContracts.Models;
using Stratis.Bitcoin.Features.Wallet.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stratis.STOPlatform.Core.Swagger
{
    public interface ISwaggerApi
    {
        [Get("/smartcontracts/receipt-search")]

        Task<List<Receipt>> GetReceiptsAsync(ReceiptSearchParameters parameter);

        [Post("/smartcontractwallet/create")]
        Task<string> DeploySmartContractAsync(DeployContractApiParameters parameter);

        [Get("/wallet/addresses?walletName={walletName}&&accountName={accountName}")]
        Task<WalletAddressResponse> GetAddressesAsync(string walletName, string accountName = "account 0");

        [Get("/wallet/list-wallets")]
        Task<WalletListResponse> GetWalletsAsync();

        [Post("/smartcontracts/local-call")]
        Task<LocalCallResponse> LocalCallAsync(LocalCallParameters parameters);

        [Get("/SmartContracts/balance?address={contractAddress}")]
        Task<decimal> GetBalanceAsync(string contractAddress);
    }
}