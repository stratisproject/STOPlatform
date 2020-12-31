using Stratis.STOPlatform.Core.Providers;
using Stratis.STOPlatform.Data.Docs;
using Stratis.STOPlatform.Data.Entities;
using System.Collections.Generic;
using System.Linq;
using Stratis.STOPlatform.Core.Extensions;
using NBitcoin;

namespace Stratis.STOPlatform.Models.DashboardViewModels
{
    public class DashboardIndexViewModel
    {
        public ContributionModel Contribution { get; }

        public bool HasAddress => !string.IsNullOrEmpty(Contribution.Address);

        public List<UserDepositViewModel> Deposits { get; set; } = new List<UserDepositViewModel>();

        public bool ShowTotalContribution { get; private set; }
        public bool STOFinished { get; set; }

        public string TokenName { get; set; }
        public string TermsUrl { get; set; }
        public decimal TotalContribution { get; set; }

        public CoinInfo CirrusInfo { get; }
        public DashboardIndexViewModel()
        {
        }

        public DashboardIndexViewModel(List<Deposit> deposits, STOSetting setting, CoinInfo cirrusInfo)
        {
            CirrusInfo = cirrusInfo;
                        
            Deposits = deposits.OrderByDescending(u => u.Date).Select(u => new UserDepositViewModel(u)).ToList();

            Contribution = new ContributionModel
            {
                CoinInfo = cirrusInfo,
                Address = setting.ContractAddress,
                Balance = Deposits.Sum(t => t.Invested),
                EarnedTokens = deposits.Sum(u => u.EarnedToken),
            };

            TokenName = setting.Symbol.ToUpper();
            ShowTotalContribution = setting.ShowTotalContribution;
            TermsUrl = setting.TermsAndConditionsUrl;
        }


        public class ContributionModel
        {
            public string Address { get; set; }
            public decimal Balance { get; set; }
            public ulong EarnedTokens { get; set; }
            public CoinInfo CoinInfo { get; set; }
        }

        public class UserDepositViewModel
        {
            public string TransactionId { get; }
            public decimal Invested { get; }
            public ulong EarnedToken { get; }
            public decimal Refunded { get; set; }
            public UserDepositViewModel(Deposit deposit)
            {
                TransactionId = deposit.TransactionId;
                Invested = Money.Satoshis(deposit.Invested).ToUnit(MoneyUnit.BTC);
                Refunded = Money.Satoshis(deposit.Refunded).ToUnit(MoneyUnit.BTC);
                EarnedToken = deposit.EarnedToken;
            }
        }
    }
}