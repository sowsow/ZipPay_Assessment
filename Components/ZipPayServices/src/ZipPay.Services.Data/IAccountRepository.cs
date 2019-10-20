using System.Collections.Generic;
using System.Threading.Tasks;
using ZipPay.Common.Data.Models;

namespace ZipPay.Services.Data
{
    public interface IAccountRepository
    {
        Task<Account> AddAsync(Account account);

        Task<Account> FindByIdAsync(long id);

        Task<IEnumerable<Account>> FindByUserIdAsync(long userId);
    }
}
