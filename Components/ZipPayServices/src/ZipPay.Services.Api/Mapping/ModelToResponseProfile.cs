using AutoMapper;
using ZipPay.Common.Data.Models;
using ZipPay.Common.Rest.Models.AccountServiceApi;
using ZipPay.Common.Rest.Models.UserServiceApi;

namespace ZipPay.Services.Api.Mapping
{
    public class ModelToResponseProfile : Profile
    {
        public ModelToResponseProfile()
        {
            CreateMap<User, UserResponse>();

            CreateMap<User[], UserResponse[]>();

            CreateMap<Account, AccountResponse>();
        }
    }
}
