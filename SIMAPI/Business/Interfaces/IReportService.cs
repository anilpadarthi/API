using SIMAPI.Data.Dto;
using SIMAPI.Data.Models;

namespace SIMAPI.Business.Interfaces
{
    public interface IReportService
    {
        Task<CommonResponse> GetMonthlyActivationsAsync(GetReportRequest request);
        Task<CommonResponse> GetMonthlyHistoryActivationsAsync(GetReportRequest request);
        Task<CommonResponse> GetDailyGivenCountAsync(GetReportRequest request);
        Task<CommonResponse> GetNetworkUsageReportAsync(GetReportRequest request);
        Task<CommonResponse> GetKPITargetReportAsync(GetReportRequest request);



        Task<CommonResponse> GetMonthlyUserActivationsAsync(GetReportRequest request);
        Task<CommonResponse> GetMonthlyAreaActivationsAsync(GetReportRequest request);
        Task<CommonResponse> GetMonthlyShopActivationsAsync(GetReportRequest request);        
        Task<CommonResponse> GetInstantActivationReportAsync(GetReportRequest request);


        Task<CommonResponse> GetSalaryReportAsync(GetReportRequest request);
        Task<CommonResponse> GetSimAllocationReportAsync(GetReportRequest request);
        Task<CommonResponse> GetAccessoriesOutstandingReportsAsync(GetReportRequest request);




    }
}
