using System.Collections.Generic;
using System.Threading.Tasks;
using ZipPay.Common.Data.Models;

namespace ZipPay.Services.Data
{
    public interface IUserRepository
    {
        Task<User> AddAsync(User user);

        Task<User> FindByIdAsync(long id);

        Task<User> FindByEmailAsync(string emailAddress);

        Task<IEnumerable<User>> ListAllAsync();
    }
}
