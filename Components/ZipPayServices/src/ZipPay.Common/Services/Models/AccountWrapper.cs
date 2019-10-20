using ZipPay.Common.Data.Models;

namespace ZipPay.Common.Services.Models
{
    public class AccountWrapper : BaseWrapper<Account>
    {
        public AccountWrapper(Account account) : base(account)
        {
        }

        public AccountWrapper(string message) : base(message)
        {
        }
    }
}
