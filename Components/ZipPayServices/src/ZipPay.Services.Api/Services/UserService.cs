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
    public class UserService : IUserService
    {
        private const decimal MinNetMonthlyIncome = 1000m;

        private readonly IUserRepository _userRepository;

        private readonly IAccountRepository _accountRepository;

        private readonly ITransactionManager _transactionManager;

        private readonly ILogger<UserService> _logger;

        public UserService(
            IUserRepository userRepository,
            IAccountRepository accountRepository,
            ITransactionManager transactionManager,
            ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _accountRepository = accountRepository;
            _transactionManager = transactionManager;
            _logger = logger;
        }

        public async Task<UserWrapper> SaveAsync(User user)
        {
            try
            {
                var isEmailUnique = await IsEmailAddressUniqueAsync(user.EmailAddress);

                if (!isEmailUnique)
                {
                    return new UserWrapper(
                        $"Email [{user.EmailAddress}] already in use.");
                }

                User result = null;

                await _transactionManager
                    .DoInTransactionAsync(
                        async () =>
                        {
                            result = await _userRepository.AddAsync(user);
                        });

                return new UserWrapper(result);
            }
            catch (Exception e)
            {
                var message = $"Unable to save User [{user.EmailAddress}]. {e}";

                _logger.LogError(message);

                return new UserWrapper(message);
            }
        }

        public async Task<UserEnumerableWrapper> ListAllAsync()
        {
            try
            {
                IEnumerable<User> result = null;

                await _transactionManager
                    .DoInTransactionAsync(
                        async () =>
                        {
                            result = await _userRepository.ListAllAsync();
                        });

                return new UserEnumerableWrapper(result);

            }
            catch (Exception e)
            {
                var message = $"Unable to list all Users. {e}";

                return new UserEnumerableWrapper(message);
            }
        }

        public async Task<UserWrapper> FindByIdAsync(long userId)
        {
            try
            {
                User result = null;

                await _transactionManager
                    .DoInTransactionAsync(
                        async () =>
                        {
                            result = await _userRepository.FindByIdAsync(userId);
                        });

                return new UserWrapper(result);
            }
            catch (Exception e)
            {
                var message = $"Unable to find User Id [{userId}]. {e}";

                _logger.LogError(message);

                return new UserWrapper(message);
            }
        }

        public async Task<bool> IsEmailAddressUniqueAsync(string emailAddress)
        {
            try
            {
                User user = null;

                await _transactionManager
                    .DoInTransactionAsync(
                        async () =>
                        {
                            user = await _userRepository.FindByEmailAsync(emailAddress);
                        });

                return user == null;
            }
            catch (Exception e)
            {
                var message = $"Unable to verify email address [{emailAddress}]. {e}";

                _logger.LogError(message);

                return false;
            }
        }

        public async Task<AccountWrapper> CreateAccountAsync(long userId, Account account)
        {
            try
            {
                var isEligible = await IsUserEligibleToCreateAccount(userId);

                if (!isEligible)
                {
                    return new AccountWrapper(
                        $"Unable to create Account [{account.Name}] for User [{userId}].");
                }

                Account result = null;

                await _transactionManager
                    .DoInTransactionAsync(
                        async () =>
                        {
                            account.CreatedByUserId = userId;

                            result = await _accountRepository.AddAsync(account);
                        });

                return new AccountWrapper(result);
            }
            catch (Exception e)
            {
                var message = $"Unable to create Account [{account.Name}] for User [{userId}]. {e}";

                _logger.LogError(message);

                return new AccountWrapper(message);
            }
        }

        public async Task<bool> IsUserEligibleToCreateAccount(long userId)
        {
            try
            {
                User existingUser = null;

                await _transactionManager
                    .DoInTransactionAsync(
                        async () =>
                        {
                            existingUser = await _userRepository.FindByIdAsync(userId);
                        });

                if (existingUser == null)
                {
                    return false;
                }

                if (existingUser.MonthlySalary - existingUser.MonthlyExpenses < MinNetMonthlyIncome)
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                var message = $"Unable to verify User[{userId}]. {e}";

                _logger.LogError(message);

                return false;
            }

            return true;
        }
    }
}
