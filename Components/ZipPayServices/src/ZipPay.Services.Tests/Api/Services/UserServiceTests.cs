using AutoFixture;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;
using ZipPay.Common.Data.Models;
using ZipPay.Services.Api.Services;
using ZipPay.Services.Data;
using ZipPay.Services.Tests.Data;

namespace ZipPay.Services.Tests.Api.Services
{
    public class UserServiceTests
    {
        private readonly IUserService _userService;

        private readonly IFixture _fixture;

        private readonly IUserRepository _userRepository;

        private readonly IAccountRepository _accountRepository;

        public UserServiceTests()
        {
            _fixture = new Fixture();

            _userRepository = Substitute.For<IUserRepository>();
            _accountRepository = Substitute.For<IAccountRepository>();
            var logger = Substitute.For<ILogger<UserService>>();
            var transactionManager = new MockTransactionManager();

            _userService = new UserService(
                _userRepository,
                _accountRepository,
                transactionManager,
                logger);
        }

        [Fact]
        public async void GivenEmailIsNotUniqueThenSaveAsyncShouldNotAddUser()
        {
            var user = _fixture.Create<User>();

            _userRepository
                .FindByEmailAsync(Arg.Any<string>())
                .Returns(user);

            await _userService.SaveAsync(user);

            await _userRepository
                .Received(0)
                .AddAsync(user);
        }

        [Fact]
        public async void GivenEmailIsValidThenSaveAsyncShouldAddUser()
        {
            var user = _fixture.Build<User>()
                .With(x => x.MonthlySalary, 1000)
                .With(x => x.MonthlyExpenses, 0)
                .Create();

            _userRepository
                .FindByEmailAsync(Arg.Any<string>())
                .Returns((User) null);

            await _userService.SaveAsync(user);

            await _userRepository
                .Received(1)
                .AddAsync(user);
        }

        [Fact]
        public async void GivenUserDoesNotExistWhenCreateAccountAsyncShouldNotAddAccount()
        {
            var user = _fixture.Create<User>();
            var account = _fixture.Create<Account>();

            _userRepository
                .FindByEmailAsync(Arg.Any<string>())
                .Returns((User)null);

            await _userService
                .CreateAccountAsync(user.Id, account);

            await _accountRepository
                .Received(0)
                .AddAsync(account);
        }

        [Theory]
        [InlineData(0, 0)]
        [InlineData(1000, 1)]
        [InlineData(999, 1000)]
        public async void GivenExistingUserNetMonthlyIncomeIsNotValidWhenCreateAccountAsyncShouldNotAddAccount(
            decimal salary,
            decimal expenses)
        {
            var user = _fixture.Build<User>()
                .With(x => x.MonthlySalary, salary)
                .With(x => x.MonthlyExpenses, expenses)
                .Create();

            _userRepository
                .FindByIdAsync(user.Id)
                .Returns(user);

            var account = _fixture.Create<Account>();

            await _userService.CreateAccountAsync(user.Id, account);

            await _accountRepository
                .Received(0)
                .AddAsync(account);
        }

        [Fact]
        public async void GivenExistingUserAndValidNetMonthlyIncomeWhenCreateAccountAsyncShouldAddAccount()
        {
            var user = _fixture.Build<User>()
                .With(x => x.MonthlySalary, 1000m)
                .With(x => x.MonthlyExpenses, 0m)
                .Create();
            var account = _fixture.Create<Account>();

            _userRepository
                .FindByIdAsync(user.Id)
                .Returns(user);

            await _userService
                .CreateAccountAsync(user.Id, account);

            await _accountRepository
                .Received(1)
                .AddAsync(Arg.Is<Account>(x => x.CreatedByUserId == user.Id));
        }
    }
}
