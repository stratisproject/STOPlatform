using Stratis.STOPlatform.Models.SetupViewModels;
using Stratis.SmartContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stratis.STOPlatform.Core.Swagger
{
    public class DeployContractParameter
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
        public ulong TotalSupply { get; set; }
        public SalePeriodParameter[] Periods { get; set; }
        public string WalletName { get; set; }
        public string ContractCode { get; set; }
        public string Password { get; set; }
        public string Sender { get; set; }
        public string Owner { get; set; }
        public int GasLimit { get; set; }
        public int GasPrice { get; set; }
        public decimal FeeAmount { get; set; }
        public TokenType TokenType { get; set; }

        public DeployContractApiParameters ToApiParameters(ISerializer serializer, string kycContractAddress, string mapperContractAddress)
        {
            var periodHex = BitConverter.ToString(serializer.Serialize(Periods)).Replace("-", string.Empty);
            return new DeployContractApiParameters
            {
                WalletName = WalletName,
                ContractCode = ContractCode,
                Password = Password,
                Sender = Sender,
                GasLimit = GasLimit,
                GasPrice = GasPrice,
                FeeAmount = FeeAmount,
                Parameters = new[] { $"9#{Owner}", $"5#{(uint)TokenType}", $"7#{TotalSupply}", $"4#{Name}", $"4#{Symbol}", $"9#{kycContractAddress}", $"9#{mapperContractAddress}", $"10#{periodHex}" }
            };
        }
    }
}
