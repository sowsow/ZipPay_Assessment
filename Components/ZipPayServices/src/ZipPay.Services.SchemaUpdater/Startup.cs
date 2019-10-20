using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZipPay.Common.Data;
using IStartup = ZipPay.Common.Hosting.IStartup;

namespace ZipPay.Services.SchemaUpdater
{
    public class Startup : IStartup
    {
        public string ApplicationName => "schemaupdater";
        
        public Action<WebHostBuilderContext, IConfigurationBuilder> ConfigureAppConfigurationDelegate => null;

        public void ConfigureApp(IApplicationBuilder app)
        {
        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddCoreDataServices(configuration)
                .AddHostedService<SchemaUpdateService<Anchor>>();
        }
    }
}
