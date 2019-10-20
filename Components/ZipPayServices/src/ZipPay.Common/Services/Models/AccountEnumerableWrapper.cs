using System.Collections.Generic;
using ZipPay.Common.Data.Models;

namespace ZipPay.Common.Services.Models
{
    public class AccountEnumerableWrapper : BaseWrapper<IEnumerable<Account>>
    {
        public AccountEnumerableWrapper(IEnumerable<Account> accounts) : base(accounts)
        {
        }

        public AccountEnumerableWrapper(string message) : base(message)
        {
        }
    }
}
