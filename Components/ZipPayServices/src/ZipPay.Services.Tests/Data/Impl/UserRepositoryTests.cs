using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Xunit;
using ZipPay.Common.Data.Models;
using ZipPay.Services.Data;

namespace ZipPay.Services.Tests.Data.Impl
{
    public class UserRepositoryTests : BaseDatabaseTests
    {
        private readonly IUserRepository _userRepository;

        private readonly IFixture _fixture;

        public UserRepositoryTests(DatabaseFixture databaseFixture)
            : base(databaseFixture)
        {
            _userRepository = (IUserRepository) CallScope
                .ServiceProvider
                .GetService(typeof(IUserRepository));

            _fixture = new Fixture();
        }

        [Fact]
        public async Task GivenValidDataWhenInsertThenResultIsCorrect()
        {
            var user = _fixture.Create<User>();

            await TransactionManager.DoInTransactionAsync(
                async () =>
                {
                    var actual = await _userRepository.AddAsync(user);

                    actual.Id.Should().Be(user.Id);
                    actual.EmailAddress.Should().Be(user.EmailAddress);
                    actual.Name.Should().Be(user.Name);
                    actual.MonthlySalary.Should().Be(user.MonthlySalary);
                    actual.MonthlyExpenses.Should().Be(user.MonthlyExpenses);
                });
        }

        [Fact]
        public async Task GivenValidDataExistsWhenFindByIdThenResultIsCorrect()
        {
            var user = _fixture.Create<User>();

            await TransactionManager.DoInTransactionAsync(
                async () =>
                {
                    await _userRepository.AddAsync(user);

                    var actual = await _userRepository.FindByIdAsync(user.Id);

                    actual.Id.Should().Be(user.Id);
                    actual.EmailAddress.Should().Be(user.EmailAddress);
                    actual.Name.Should().Be(user.Name);
                    actual.MonthlySalary.Should().Be(user.MonthlySalary);
                    actual.MonthlyExpenses.Should().Be(user.MonthlyExpenses);
                });
        }

        [Fact]
        public async Task GivenValidDataExistsWhenFindByEmailThenResultIsCorrect()
        {
            var user = _fixture.Create<User>();

            await TransactionManager.DoInTransactionAsync(
                async () =>
                {
                    await _userRepository.AddAsync(user);

                    var actual = await _userRepository.FindByEmailAsync(user.EmailAddress);

                    actual.Id.Should().Be(user.Id);
                    actual.EmailAddress.Should().Be(user.EmailAddress);
                    actual.Name.Should().Be(user.Name);
                    actual.MonthlySalary.Should().Be(user.MonthlySalary);
                    actual.MonthlyExpenses.Should().Be(user.MonthlyExpenses);
                });
        }

        [Fact]
        public async Task GivenValidDataExistsWhenListAllThenResultIsCorrect()
        {
            var users = _fixture.CreateMany<User>().ToList();

            await TransactionManager.DoInTransactionAsync(
                async () =>
                {
                    foreach (var user in users)
                    {
                        await _userRepository.AddAsync(user);
                    }

                    var actual = await _userRepository.ListAllAsync();

                    actual.Should().HaveCount(users.Count());
                });
        }
    }
}
