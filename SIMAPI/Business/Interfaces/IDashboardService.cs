using SIMAPI.Data.Dto;
using SIMAPI.Data.Models;

namespace SIMAPI.Business.Interfaces
{
    public interface IDashboardService
    {

        Task<CommonResponse> GetAreaWiseActivationsAsync(GetReportRequest request);
        Task<CommonResponse> GetUserWiseActivationsAsync(GetReportRequest request);
        Task<CommonResponse> GetNetworkWiseActivationsAsync(GetReportRequest request);
        Task<CommonResponse> GetUserWiseKPIReportAsync(GetReportRequest request);
        Task<CommonResponse> GetSimAllocationReportAsync(GetReportRequest request);
        Task<CommonResponse> GetDahboardMetricsAsync(GetReportRequest request);
        Task<CommonResponse> GetDahboardAccessoriesMetricsAsync(GetReportRequest request);
        Task<CommonResponse> GetDahboardChartActivationMetricsAsync(GetReportRequest request);


    }
}
