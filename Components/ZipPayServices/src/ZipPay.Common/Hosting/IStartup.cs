using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ZipPay.Common.Hosting
{
    public interface IStartup
    {
        string ApplicationName { get; }

        Action<WebHostBuilderContext, IConfigurationBuilder> ConfigureAppConfigurationDelegate { get; }

        void ConfigureServices(IServiceCollection services, IConfiguration configuration);

        void ConfigureApp(IApplicationBuilder app);
    }
}