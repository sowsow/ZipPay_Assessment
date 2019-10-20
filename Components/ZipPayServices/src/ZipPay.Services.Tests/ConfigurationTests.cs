using System;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using ZipPay.Common.Hosting;

namespace ZipPay.Services.Tests
{
    public class ConfigurationTests
    {
        [Theory]
        [InlineData("schemaupdater", typeof(SchemaUpdater.Startup))]
        [InlineData("api", typeof(Services.Api.Startup))]
        public void WhenServicesAreAddedThenAllTheInterfacesCanBeResolved(
            string role,
            Type type,
            Type additionalType = null)
        {
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            var serviceCollection = new ServiceCollection()
                .AddLogging()
                .AddEssentials();

            var startup = Activator.CreateInstance(type) as IStartup;
            startup?.ApplicationName.Should().Be(role);
            startup?.ConfigureServices(serviceCollection, configuration);

            var serviceProvider = serviceCollection.BuildServiceProvider();

            var avoidInterfaces = new Type[0];

            var interfaces = type.Assembly.GetTypes()
                .Where(c => c.IsInterface && c.IsPublic)
                .Where(c => avoidInterfaces.All(i => c != i))
                .ToArray();

            foreach (var assembly in interfaces)
            {
                serviceProvider.GetServices(assembly).Should().NotBeEmpty($"{assembly.Name} can't be resolved");
            }

            if (additionalType == null)
            {
                return;
            }

            serviceProvider.GetServices(additionalType).Should().NotBeEmpty();
        }
    }
}
