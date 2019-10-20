using System.Threading.Tasks;
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
    public class AccountServiceTests
    {
        private readonly IAccountService _accountService;

        private readonly IFixture _fixture;

        private readonly IAccountRepository _accountRepository;

        private readonly IUserService _userService;

        public AccountServiceTests()
        {
            _fixture = new Fixture();

            _accountRepository = Substitute.For<IAccountRepository>();
            _userService = Substitute.For<IUserService>();
            var logger = Substitute.For<ILogger<UserService>>();
            var transactionManager = new MockTransactionManager();

            _accountService = new AccountService(
                _accountRepository,
                _userService,
                transactionManager,
                logger);
        }

        [Fact]
        public async Task WhenUserIsNotEligibleThenSaveAsyncShouldNotAddAccount()
        {
            var account = _fixture.Create<Account>();

            _userService
                .IsUserEligibleToCreateAccount(account.CreatedByUserId)
                .Returns(false);

            await _accountService.SaveAsync(account);

            await _accountRepository
                .Received(0)
                .AddAsync(account);
        }


        [Fact]
        public async Task WhenUserIsEligibleThenSaveAsyncShouldAddAccount()
        {
            var account = _fixture.Create<Account>();

            _userService
                .IsUserEligibleToCreateAccount(account.CreatedByUserId)
                .Returns(true);

            await _accountService.SaveAsync(account);

            await _accountRepository
                .Received(1)
                .AddAsync(account);
        }
    }
}
