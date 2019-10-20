using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Xunit;
using ZipPay.Common.Data.Models;
using ZipPay.Services.Data;

namespace ZipPay.Services.Tests.Data.Impl
{
    public class AccountRepositoryTests : BaseDatabaseTests
    {
        private readonly IAccountRepository _accountRepository;

        private readonly IFixture _fixture;

        public AccountRepositoryTests(DatabaseFixture databaseFixture)
            : base(databaseFixture)
        {
            _accountRepository = (IAccountRepository) CallScope
                .ServiceProvider
                .GetService(typeof(IAccountRepository));

            _fixture = new Fixture();
        }

        [Fact]
        public async Task GivenValidDataWhenInsertThenResultIsCorrect()
        {
            var account = _fixture.Create<Account>();

            await TransactionManager.DoInTransactionAsync(
                async () =>
                {
                    var actual = await _accountRepository.AddAsync(account);

                    actual.Id.Should().Be(account.Id);
                    actual.Name.Should().Be(account.Name);
                    actual.CreatedByUserId.Should().Be(account.CreatedByUserId);
                });
        }

        [Fact]
        public async Task GivenValidDataExistsWhenFindByIdThenResultIsCorrect()
        {
            var account = _fixture.Create<Account>();

            await TransactionManager.DoInTransactionAsync(
                async () =>
                {
                    await _accountRepository.AddAsync(account);

                    var actual = await _accountRepository.FindByIdAsync(account.Id);

                    actual.Id.Should().Be(account.Id);
                    actual.Name.Should().Be(account.Name);
                    actual.CreatedByUserId.Should().Be(account.CreatedByUserId);
                });
        }

        [Fact]
        public async Task GivenValidDataExistsWhenFindByUserIdThenResultIsCorrect()
        {
            var userId = _fixture.Create<long>();
            var account = _fixture.Build<Account>()
                .With(x => x.CreatedByUserId, userId)
                .Create();

            await TransactionManager.DoInTransactionAsync(
                async () =>
                {
                    await _accountRepository.AddAsync(account);

                    var actual = (await _accountRepository.FindByUserIdAsync(account.CreatedByUserId)).ToArray();

                    actual.Length.Should().Be(1);
                    actual.Single().Id.Should().Be(account.Id);
                    actual.Single().Name.Should().Be(account.Name);
                    actual.Single().CreatedByUserId.Should().Be(account.CreatedByUserId);
                });
        }
    }
}
