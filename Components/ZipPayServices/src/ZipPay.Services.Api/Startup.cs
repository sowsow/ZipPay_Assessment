using System;
using System.Reflection;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZipPay.Services.Api.Configurations;
using ZipPay.Services.Api.Services;
using IStartup = ZipPay.Common.Hosting.IStartup;

namespace ZipPay.Services.Api
{
    public class Startup : IStartup
    {
        public string ApplicationName => "api";

        public Action<WebHostBuilderContext, IConfigurationBuilder> ConfigureAppConfigurationDelegate => null;

        public void ConfigureApp(IApplicationBuilder app)
        {
            app
                .UseMvc()
                .UseSwagger()
                .UseSwaggerUi();
        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddSwagger()
                .AddApiDataServices(configuration)
                .AddAutoMapper(Assembly.GetExecutingAssembly())
                .AddMvc(options => options.RespectBrowserAcceptHeader = true)
                .AddApplicationPart(typeof(Startup).Assembly)
                .AddJsonOptions(
                    options =>
                    {
                        options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                    })
                .AddXmlSerializerFormatters();
        }
    }
}