using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Stratis.STOPlatform.Data;
using Stratis.STOPlatform.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stratis.STOPlatform.Core.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext database;

        public UserService(ApplicationDbContext database)
        {
            this.database = database;
        }

        public async Task<User> UpsertAsync(string identityAddress, string primaryAddress, string fullname)
        {
            var user = await database.Users.FirstOrDefaultAsync(u => u.Address == identityAddress);

            if (user == null)
            {
                user = new User { FullName = fullname, Address = identityAddress, WalletAddress = primaryAddress };
                database.Users.Add(user);
            }
            else
            {

                user.FullName = fullname;
                user.WalletAddress = primaryAddress;
            }

            await database.SaveChangesAsync();

            return user;
        }
    }
}
