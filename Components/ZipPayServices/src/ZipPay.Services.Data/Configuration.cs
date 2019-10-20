using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZipPay.Common.Data;
using ZipPay.Services.Data.Impl;

namespace ZipPay.Services.Data
{
    public static class Configuration
    {
        public static IServiceCollection AddDataServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddCoreDataServices(configuration)
                .AddTransient<IUserRepository, UserRepository>()
                .AddTransient<IAccountRepository, AccountRepository>();

            return services;
        }
    }
}
