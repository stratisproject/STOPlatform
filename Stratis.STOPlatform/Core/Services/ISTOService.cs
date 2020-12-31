using NBitcoin;
using Stratis.STOPlatform.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stratis.STOPlatform.Core.Services
{
    public interface ISTOService
    {
        Task<List<Deposit>> GetUpdatedDepositsAsync(int userId);
    }
}