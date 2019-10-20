using ZipPay.Common.Data.Models;

namespace ZipPay.Common.Services.Models
{
    public class UserWrapper : BaseWrapper<User>
    {
        public UserWrapper(User user) : base(user)
        {
        }

        public UserWrapper(string message) : base(message)
        {
        }
    }
}
