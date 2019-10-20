using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ZipPay.Common.Data;
using ZipPay.Common.Data.Models;
using ZipPay.Common.Services.Models;
using ZipPay.Services.Data;

namespace ZipPay.Services.Api.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        private readonly IUserService _userService;

        private readonly ITransactionManager _transactionManager;

        private readonly ILogger<UserService> _logger;

        public AccountService(
            IAccountRepository accountRepository,
            IUserService userService,
            ITransactionManager transactionManager,
            ILogger<UserService> logger)
        {
            _accountRepository = accountRepository;
            _logger = logger;
            _transactionManager = transactionManager;
            _userService = userService;
        }

        public async Task<AccountWrapper> SaveAsync(Account account)
        {
            try
            {
                var isEligible = false;

                await _transactionManager
                    .DoInTransactionAsync(
                        async () =>
                        {
                            isEligible = await _userService
                                .IsUserEligibleToCreateAccount(account.CreatedByUserId);
                        });

                if (!isEligible)
                {
                    return new AccountWrapper(
                        $"User [{account.CreatedByUserId}] is not eligible to create account");
                }

                var rv = await _accountRepository.AddAsync(account);

                return new AccountWrapper(rv);
            }
            catch (Exception e)
            {
                var message = $"Unable to save Account Name [{account.Name}]. {e}";

                _logger.LogError(message);

                return new AccountWrapper(message);
            }
        }

        public async Task<AccountWrapper> FindByIdAsync(long id)
        {
            try
            {
                Account result = null;

                await _transactionManager
                    .DoInTransactionAsync(
                        async () =>
                        {
                            result = await _accountRepository.FindByIdAsync(id);
                        });

                return new AccountWrapper(result);
            }
            catch (Exception e)
            {
                var message = $"Unable to find Account Id [{id}]. {e}";

                _logger.LogError(message);

                return new AccountWrapper(message);
            }
        }

        public async Task<AccountEnumerableWrapper> FindByUserIdAsync(long userId)
        {
            try
            {
                IEnumerable<Account> result = null;

                await _transactionManager
                    .DoInTransactionAsync(
                        async () =>
                        {
                            result = await _accountRepository.FindByUserIdAsync(userId);
                        });

                return new AccountEnumerableWrapper(result);
            }
            catch (Exception e)
            {
                var message = $"Unable to find Accounts for User Id [{userId}]. {e}";

                _logger.LogError(message);

                return new AccountEnumerableWrapper(message);
            }
        }
    }
}
