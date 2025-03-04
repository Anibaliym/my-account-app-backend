using MyAccountApp.Core.Entities;

namespace MyAccountApp.Core.Interfaces
{
    public interface IAccountRepository
    {
        Task<Account> GetActiveAccountById(Guid id);
        Task<IEnumerable<Account>> GetActiveAccountByUserId(Guid userId);
        Task<int> GetTotalUserAccounts(Guid userId);
        Task<int> GetNextAccountOrderByUserId(Guid userId);
        Task CreateAccount(Account model);
        Task UpdateAccount(Account model);
        Task<bool> DeleteAccount(Guid id);
    }
}
