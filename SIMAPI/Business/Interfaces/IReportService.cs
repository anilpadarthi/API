using SIMAPI.Data.Dto;
using SIMAPI.Data.Models;
using SIMAPI.Data.Models.Report;

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
        Task<CommonResponse> GetMonthlyAccessoriesReportAsync(GetReportRequest request);
        Task<CommonResponse> GetMonthlyAccessoriesCommissionPercentReportAsync(GetReportRequest request);

        Task<CommonResponse> GetChequeWithdrawnReportsAsync(GetReportRequest request);
        Task<CommonResponse> GetBankChequeStatusAsync(string chequeNumber);



    }
}
