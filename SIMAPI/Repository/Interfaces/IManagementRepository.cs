using SIMAPI.Data.Entities;

namespace SIMAPI.Repository.Interfaces
{
    public interface IManagementRepository : IRepository
    {
        Task<UserSalaryTransaction?> GetUserSalaryTransactionAsync(int id);
        Task<IEnumerable<UserSalaryTransaction>> GetUserSalaryTransactionsAsync(int userId, DateTime date);
    }
}
