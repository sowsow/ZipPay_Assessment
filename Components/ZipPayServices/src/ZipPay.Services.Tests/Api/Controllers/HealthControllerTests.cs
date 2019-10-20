using System;
using FluentAssertions;
using Xunit;
using ZipPay.Services.Api.Controllers;

namespace ZipPay.Services.Tests.Api.Controllers
{
    public class HealthControllerTests
    {
        private readonly HealthController _healthController;

        public HealthControllerTests()
        {
            _healthController = new HealthController(() => new DateTime(2019, 10, 30));
        }

        [Fact]
        public void TestPing()
        {
            var result = _healthController.Ping();

            result.Should().NotBeEmpty();
        }
    }
}
