using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ZipPay.Services.Tests
{
    public class HostTests
    {
        [Theory]
        [InlineData("api")]
        [InlineData("schemaupdater")]
        public void TestApps(string role)
        {
            Task.WaitAll(GetActions(role).ToArray());
        }

        private static IEnumerable<Task> GetActions(string role)
        {
            Environment.SetEnvironmentVariable("APPLICATIONNAME", role);

            yield return Task.Factory.StartNew(
                async () =>
                {
                    await Program.Main();
                });

            yield return Task.Factory.StartNew(
                async () =>
                {
                    Thread.Sleep(5000);
                    await Program.Host.StopAsync();
                });
        }
    }
}
