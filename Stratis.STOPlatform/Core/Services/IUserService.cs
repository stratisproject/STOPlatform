using Stratis.STOPlatform.Data.Entities;
using System.Threading.Tasks;

namespace Stratis.STOPlatform.Core.Services
{
    public interface IUserService
    {
        Task<User> UpsertAsync(string IdentityAddress, string primaryAddress, string fullname);
    }
}