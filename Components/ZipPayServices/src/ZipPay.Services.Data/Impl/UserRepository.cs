using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using ZipPay.Common.Data;
using ZipPay.Common.Data.Models;

namespace ZipPay.Services.Data.Impl
{
    public class UserRepository : IUserRepository
    {
        private readonly ITransactionManager _transactionManager;

        public UserRepository(ITransactionManager transactionManager)
        {
            _transactionManager = transactionManager;
        }

        public async Task<User> AddAsync(User user)
        {
            return await _transactionManager
                .GetCurrentConnection()
                .QuerySingleOrDefaultAsync<User>(
                    Queries.Queries.InsertUser,
                    new
                    {
                        id = user.Id,
                        emailaddress = user.EmailAddress,
                        name = user.Name,
                        monthlysalary = user.MonthlySalary,
                        monthlyexpenses = user.MonthlyExpenses
                    });
        }

        public async Task<User> FindByIdAsync(long id)
        {
            return await _transactionManager
                .GetCurrentConnection()
                .QuerySingleOrDefaultAsync<User>(
                    Queries.Queries.FindUserById,
                    new { id });
        }

        public async Task<User> FindByEmailAsync(string emailAddress)
        {
            return await _transactionManager
                .GetCurrentConnection()
                .QuerySingleOrDefaultAsync<User>(
                    Queries.Queries.FindUserByEmailAddress,
                    new { emailAddress });
        }

        public async Task<IEnumerable<User>> ListAllAsync()
        {
            return await _transactionManager
                .GetCurrentConnection()
                .QueryAsync<User>(
                    Queries.Queries.FindAllUsers);
        }
    }
}
