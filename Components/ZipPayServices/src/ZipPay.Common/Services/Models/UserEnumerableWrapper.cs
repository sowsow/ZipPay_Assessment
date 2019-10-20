using System.Collections.Generic;
using ZipPay.Common.Data.Models;

namespace ZipPay.Common.Services.Models
{
    public class UserEnumerableWrapper : BaseWrapper<IEnumerable<User>>
    {
        public UserEnumerableWrapper(IEnumerable<User> users) : base(users)
        {
        }

        public UserEnumerableWrapper(string message) : base(message)
        {
        }
    }
}
