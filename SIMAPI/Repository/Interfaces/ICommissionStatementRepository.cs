using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models.CommissionStatement;

namespace SIMAPI.Repository.Interfaces
{
    public interface ICommissionStatementRepository : IRepository
    {
        Task<ShopCommissionHistory?> GetCommissionHistoryDetailsAsync(int shopCommissionHistoryId);
        Task<IEnumerable<ShopCommissionHistory>> GetCommissionHistoryListAsync(string referenceNumber);
        Task<IEnumerable<CommissionListModel>> GetCommissionListAsync(GetReportRequest request);
        Task<IEnumerable<CommissionListModel>> GetAreaCommissionListAsync(GetReportRequest request);
        Task<IEnumerable<CommissionShopListModel>> GetCommissionShopList(GetReportRequest request);
        Task<IEnumerable<ExportCommissionList>> ExportCommissionChequeExcelAsync(GetReportRequest request);
        Task<IEnumerable<CommissionStatementModel?>> GetCommissionStatementAsync(GetReportRequest request);
        Task<ShopWalletHistory?> GetShopBonusHistoryByReferenceNumber(int shopCommissionHistoryId);
    }
}
