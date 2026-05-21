using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models.OrderListModels;

namespace SIMAPI.Repository.Interfaces
{
    public interface IManagementRepository : IRepository
    {
        Task<UserSalaryTransaction?> GetUserSalaryTransactionAsync(int id);
        Task<IEnumerable<UserSalaryTransaction>> GetUserSalaryTransactionsAsync(int userId, DateTime date);
        Task<IEnumerable<AccessoriesOutstandingBalanceModel>> OutStandingAccessoriesReportAsync(GetReportRequest request);
        Task<SalaryCommissionConfiguration?> GetCommissionConfigurationAsync(SalaryCommissionConfiguration model);
        Task<Configuration?> GetConfigurationAsync();
    }
}
