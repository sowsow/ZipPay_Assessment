using System.Threading.Tasks;
using ZipPay.Common.Data.Models;
using ZipPay.Common.Services.Models;

namespace ZipPay.Services.Api.Services
{
    public interface IAccountService
    {
        Task<AccountWrapper> SaveAsync(Account account);

        Task<AccountWrapper> FindByIdAsync(long id);

        Task<AccountEnumerableWrapper> FindByUserIdAsync(long userId);
    }
}
