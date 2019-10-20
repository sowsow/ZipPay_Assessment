using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using ZipPay.Common.Data;
using ZipPay.Common.Data.Models;

namespace ZipPay.Services.Data.Impl
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ITransactionManager _transactionManager;
        
        public AccountRepository(ITransactionManager transactionManager)
        {
            _transactionManager = transactionManager;
        }

        public async Task<Account> AddAsync(Account account)
        {
            return await _transactionManager
                .GetCurrentConnection()
                .QuerySingleOrDefaultAsync<Account>(
                    Queries.Queries.InsertAccount,
                    new
                    {
                        id = account.Id,
                        name = account.Name,
                        createdbyuserid = account.CreatedByUserId
                    });
        }

        public async Task<Account> FindByIdAsync(long id)
        {
            return await _transactionManager
                .GetCurrentConnection()
                .QuerySingleOrDefaultAsync<Account>(
                    Queries.Queries.FindAccountById,
                    new { id });
        }

        public async Task<IEnumerable<Account>> FindByUserIdAsync(long userId)
        {
            return await _transactionManager
                .GetCurrentConnection()
                .QueryAsync<Account>(
                    Queries.Queries.FindAccountsByUserId,
                    new { createdbyuserid = userId });
        }
    }
}
