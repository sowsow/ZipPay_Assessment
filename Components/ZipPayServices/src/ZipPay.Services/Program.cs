using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using ZipPay.Common.Hosting;
using ZipPay.Services.Api;

namespace ZipPay.Services
{
    public static class Program
    {
        public static IWebHost Host { get; private set; }

        public static async Task Main()
        {
            Host = new ExtendedHostBuilder()
                .Use<Startup>()
                .Use<SchemaUpdater.Startup>()
                .Build();

            await Host.RunAsync();
        }
    }
}
