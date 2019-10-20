using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZipPay.Services.Data;

namespace ZipPay.Services.Api.Services
{
    public static class Configuration
    {
        public static IServiceCollection AddApiDataServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddDataServices(configuration)
                .AddTransient<IUserService, UserService>()
                .AddTransient<IAccountService, AccountService>();

            return services;
        }
    }
}
