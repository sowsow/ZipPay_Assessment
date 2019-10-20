using System.Threading.Tasks;
using ZipPay.Common.Data.Models;
using ZipPay.Common.Services.Models;

namespace ZipPay.Services.Api.Services
{
    public interface IUserService
    {
        Task<UserWrapper> SaveAsync(User user);

        Task<UserWrapper> FindByIdAsync(long userId);

        Task<UserEnumerableWrapper> ListAllAsync();

        Task<bool> IsEmailAddressUniqueAsync(string emailAddress);

        Task<AccountWrapper> CreateAccountAsync(long userId, Account account);

        Task<bool> IsUserEligibleToCreateAccount(long userId);
    }
}
