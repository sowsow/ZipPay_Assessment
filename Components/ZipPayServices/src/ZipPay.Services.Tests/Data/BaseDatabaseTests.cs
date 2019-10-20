using System;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using ZipPay.Common.Data;

namespace ZipPay.Services.Tests.Data
{
    public class BaseDatabaseTests : IClassFixture<DatabaseFixture>, IDisposable
    {
        private readonly DatabaseFixture _databaseFixture;

        public BaseDatabaseTests(DatabaseFixture databaseFixture)
        {
            _databaseFixture = databaseFixture;
            CallScope = databaseFixture.ServiceProvider.CreateScope();
            TransactionManager = CallScope.ServiceProvider.GetService<ITransactionManager>();
        }

        ~BaseDatabaseTests()
        {
            Dispose(false);
        }

        protected IServiceScope CallScope { get; }

        protected ITransactionManager TransactionManager { get; }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            var tables = new[]
            {
                "zipuser",
                "account"
            };

            _databaseFixture.ResetDataInTestDatabase(tables);
            CallScope.Dispose();
        }
    }
}