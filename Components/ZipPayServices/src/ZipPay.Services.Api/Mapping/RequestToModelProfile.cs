using AutoMapper;
using ZipPay.Common.Data.Models;
using ZipPay.Common.Rest.Models.AccountServiceApi;
using ZipPay.Common.Rest.Models.UserServiceApi;

namespace ZipPay.Services.Api.Mapping
{
    public class RequestToModelProfile : Profile
    {
        public RequestToModelProfile()
        {
            CreateMap<CreateUserRequest, User>();

            CreateMap<CreateAccountRequest, Account>();
        }
    }
}
