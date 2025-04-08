using SIMAPI.Data.Dto;
using SIMAPI.Data.Models;
using SIMAPI.Data.Models.CommissionStatement;

namespace SIMAPI.Business.Interfaces
{
    public interface ICommissionStatementService
    {
        Task<CommonResponse> GetCommissionHistoryDetailsAsync(int shopCommissionHistoryId);
        Task<CommonResponse> OptInForShopCommissionAsync(int shopCommissionHistoryId,string optInType,int userId);
        Task<CommonResponse> GetCommissionListAsync(GetReportRequest request);
        Task<IEnumerable<ExportCommissionList>> ExportCommissionChequeExcelAsync(GetReportRequest request);
        Task<byte[]> DownloadPDFStatementReportAsync(GetReportRequest request);
        Task<byte[]> DownloadVATStatementReportAsync(GetReportRequest request);
        Task<CommonResponse> HideBonusAsync(int shopCommissionHistoryId, bool isDisplayBonus);
    }
}
