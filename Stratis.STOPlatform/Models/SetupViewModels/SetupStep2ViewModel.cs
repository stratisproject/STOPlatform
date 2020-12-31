using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Mono.Cecil;
using NBitcoin;
using Stratis.STOPlatform.Core;
using Stratis.STOPlatform.Core.Swagger;
using Stratis.STOPlatform.Data.Docs;
using Stratis.SmartContracts.CLR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Stratis.STOPlatform.Models.SetupViewModels
{
    public class SetupStep2ViewModel
    {
        [Required]
        [StringLength(20)]
        [Display(Name = "Token Name")]
        public string Name { get; set; } = "My Token";

        [Required]
        [StringLength(10)]
        public string Symbol { get; set; } = "MTK";

        [Required]
        [Display(Name = "Total Supply")]
        public ulong TotalSupply { get; set; } = 21_000_000;

        public List<SalePeriod> Periods { get; set; } = new List<SalePeriod>();

        [DataType(DataType.Password)]
        [Required]
        [Display(Name = "Wallet Password")]
        public string WalletPassword { get; set; }

        [Display(Name = "Fee Amount")]
        public decimal FeeAmount { get; set; } = 0.001m;

        [Display(Name = "Gas Price")]
        public int GasPrice { get; set; } = 100;

        [Display(Name = "Gas Limit")]
        public int GasLimit { get; set; } = 100_000;

        [Required]
        [Display(Name = "Wallet Name")]
        [StringLength(30)]
        public string WalletName { get; set; }

        [Required]
        public string Sender { get; set; }

        [Required]
        [Display(Name = "Contract Owner")]
        [StringLength(34, MinimumLength = 34, ErrorMessage = "The field {0} must be 34 characters.")]
        [Remote(action: "ValidateOwner", controller: "Setup", ErrorMessage = "The {0} is invalid.")]
        public string Owner { get; set; }

        public string ByteCode => AppConstants.ContractByteCode;
        public IEnumerable<SelectListItem> Wallets { get; set; }
        public IEnumerable<WalletAddress> Addresses { get; set; } = Enumerable.Empty<WalletAddress>();

        [Display(Name = "Token")]
        [Required]
        public TokenType TokenType { get; set; }
        public void UpdateSetting(STOSetting setting, string transactionId, Network network)
        {
            setting.ContractAddress = GetContractAddress(transactionId, 0, network);
            setting.TokenAddress = GetContractAddress(transactionId, 1, network);
            setting.TotalSupply = TotalSupply;
            setting.Name = $"{Name} STO";
            setting.TokenName = Name;
            setting.Symbol = Symbol;
            setting.Owner = Owner;
        }

        private string GetContractAddress(string transactionId, ulong index, Network network)
        {
            return new AddressGenerator().GenerateAddress(new uint256(transactionId), index).ToBase58Address(network);
        }

        public DeployContractParameter ToContractParameter()
        {
            return new DeployContractParameter
            {
                Name = Name,
                Symbol = Symbol,
                TotalSupply = TotalSupply,
                TokenType = TokenType,
                Periods = Periods.Select(s => new SalePeriodParameter { Duration = s.Duration, TokenPrice = Money.Coins(s.TokenPrice) }).ToArray(),
                WalletName = WalletName,
                ContractCode = ByteCode,
                Password = WalletPassword,
                Sender = Sender,
                Owner = Owner,
                GasLimit = GasLimit,
                GasPrice = GasPrice,
                FeeAmount = FeeAmount
            };
        }

        public class SalePeriod
        {
            public uint Duration { get; set; } = 1000;

            /// <summary>
            /// Gets or sets price of token as CRS
            /// </summary>
            public decimal TokenPrice { get; set; } = 0.001m;
        }

        public class WalletAddress
        {
            public string Address { get; set; }
            public string Wallet { get; set; }
            public decimal Amount { get; set; }
        }
    }
}
