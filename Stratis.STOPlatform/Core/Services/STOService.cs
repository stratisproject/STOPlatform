using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using NBitcoin;
using Stratis.STOPlatform.Core.Providers;
using Stratis.STOPlatform.Core.Swagger;
using Stratis.STOPlatform.Data;
using Stratis.STOPlatform.Data.Docs;
using Stratis.STOPlatform.Data.Entities;
using Stratis.Sidechains.Networks;
using Stratis.SmartContracts.CLR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stratis.STOPlatform.Core.Services
{
    public class STOService : ISTOService
    {
        public readonly static TimeSpan UpdateDuration = TimeSpan.FromMinutes(1);
        private readonly ApplicationDbContext database;
        private readonly ISwaggerClient client;
        private readonly IMemoryCache cache;
        private readonly ICoinInfoProvider coinInfoProvider;

        public STOService(
            ApplicationDbContext database,
            ISwaggerClient client,
            IMemoryCache cache,
            ICoinInfoProvider coinInfoProvider
            )
        {
            this.database = database;
            this.client = client;
            this.cache = cache;
            this.coinInfoProvider = coinInfoProvider;
        }

        public async Task<List<Deposit>> GetUpdatedDepositsAsync(int userId)
        {
            var user = await database.Users
                                   .Include(u => u.Deposits)
                                   .SingleOrDefaultAsync(u => u.Id == userId);

            var stoSetting = await database.Documents.GetAsync<STOSetting>();

            if (user.LastCheck.Add(UpdateDuration) > DateTime.UtcNow)
                return user.Deposits;

            user.LastCheck = DateTime.UtcNow;
            try
            {
                await database.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return user.Deposits;
            }

            var hexAddress = user.WalletAddress.ToAddress(coinInfoProvider.Cirrus.Network).ToString();

            var receipts = await client.GetReceiptsAsync(stoSetting.ContractAddress, "InvestLog", new[] { hexAddress });

            UpdateDeposits(user, receipts);
            await database.SaveChangesAsync();

            return user.Deposits;
        }

        public void UpdateDeposits(User user, IEnumerable<Receipt> receipts)
        {
            var deposits = user.Deposits;
            //Remove transactions which is no longer exist on chain
            var removedDeposits = deposits.Where(d => !receipts.Any(r => r.TransactionHash == d.TransactionId)).ToArray();
            database.Deposits.RemoveRange(removedDeposits);

            var newReceipts = receipts.Where(r => !user.Deposits.Any(d => d.TransactionId == r.TransactionHash)).ToArray();

            foreach (var receipt in newReceipts)
            {
                var log = receipt.Logs[1].Log;

                var deposit = new Deposit
                {
                    Invested = log.Invested,
                    Refunded = log.Refunded,
                    EarnedToken = log.TokenAmount,
                    TransactionId = receipt.TransactionHash,
                    Date = DateTime.UtcNow,
                    UserId = user.Id
                };

                database.Deposits.Add(deposit);
            }

            if (newReceipts.Any() || removedDeposits.Any()) //if transactions are modified
            {
                cache.Remove(CacheKeys.Balance);
            }
        }
    }
}
