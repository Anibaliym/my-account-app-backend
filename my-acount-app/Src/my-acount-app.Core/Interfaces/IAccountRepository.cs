using MyAccountApp.Core.Entities;

namespace MyAccountApp.Core.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account> GetAccountById(Guid id);
        Task<IEnumerable<Account>> GetAccountByUserId(Guid userId);
        Task<int> GetTotalUserAccounts(Guid userId);
        Task<int> GetNextAccountOrderByUserId(Guid userId);
        Task CreateAccount(Account model);
        Task UpdateAccount(Account model);
        Task<bool> DeleteAccount(Guid id);
    }
}
