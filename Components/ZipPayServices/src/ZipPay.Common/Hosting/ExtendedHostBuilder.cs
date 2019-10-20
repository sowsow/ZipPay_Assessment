using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;

namespace ZipPay.Common.Hosting
{
    public class ExtendedHostBuilder
    {
        private const string EnvironmentName = "ASPNETCORE_ENVIRONMENT";

        private const string ApplicationName = "APPLICATIONNAME";

        private readonly Dictionary<string, IStartup> _entryPoints;

        private string _applicationName;

        public ExtendedHostBuilder()
        {
            _entryPoints = new Dictionary<string, IStartup>();
        }

        public ExtendedHostBuilder Use<T>()
            where T : IStartup, new()
        {
            var startup = new T();
            var applicationName = startup.ApplicationName;

            if (_entryPoints.ContainsKey(applicationName))
            {
                _entryPoints[applicationName] = startup;
            }
            else
            {
                _entryPoints.Add(applicationName, startup);
            }

            return this;
        }

        public ExtendedHostBuilder SetApplicationName(string applicationName)
        {
            _applicationName = applicationName;

            return this;
        }


        public IWebHost Build()
        {
            var environmentName = Environment.GetEnvironmentVariable(EnvironmentName);
            var applicationName = string.IsNullOrEmpty(_applicationName) ?
                Environment.GetEnvironmentVariable(ApplicationName) :
                _applicationName;

            if (!_entryPoints.ContainsKey(applicationName ?? throw new InvalidOperationException()))
            {
                throw new InvalidOperationException($"Entry point [{applicationName}] not found!");
            }

            var startup = _entryPoints[applicationName];

            var webHost = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .ConfigureAppConfiguration(
                    (context, configurationBuilder) =>
                    {
                        configurationBuilder
                            .AddEnvironmentVariables(string.Empty)
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile("appsettings.json")
                            .AddJsonFile($"appsettings.{environmentName}.json", true);

                        startup
                            .ConfigureAppConfigurationDelegate?.Invoke(context, configurationBuilder);
                    })
                .ConfigureLogging(
                    (context, loggingBuilder) =>
                    {
                        loggingBuilder
                            .AddConfiguration(context.Configuration.GetSection("Logging"))
                            .AddConsole(x => x.DisableColors = false);
                    })
                .ConfigureServices(
                    (context, services) =>
                    {
                        services.AddEssentials();

                        var action = _entryPoints[applicationName];

                        action.ConfigureServices(services, context.Configuration);
                    })
                .Configure(
                    app =>
                    {
                        startup.ConfigureApp(app);
                    })
                .Build();

            return webHost;
        }
    }
}
