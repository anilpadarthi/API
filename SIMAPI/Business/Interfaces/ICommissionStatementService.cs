using SIMAPI.Data.Dto;
using SIMAPI.Data.Models;

namespace SIMAPI.Business.Interfaces
{
    public interface ICommissionStatementService
    {
        Task<CommonResponse> GetCommissionListAsync(GetReportRequest request);
        Task<byte[]> DownloadPDFStatementReportAsync(GetReportRequest request);
        Task<byte[]> DownloadVATStatementReportAsync(GetReportRequest request);
    }
}
