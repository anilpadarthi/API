﻿using SIMAPI.Data.Dto;
using SIMAPI.Data.Models.Dashboard;

namespace SIMAPI.Repository.Interfaces
{
    public interface IDashboardRepository : IRepository
    {
        Task<IEnumerable<AreaWiseActivationReportModel>> GetAreaWiseActivationsAsync(GetReportRequest request);
        Task<IEnumerable<UserWiseActivationReportModel>> GetUserWiseActivationsAsync(GetReportRequest request);
        Task<IEnumerable<UserWiseKPIReportModel>> GetUserWiseKPIReportAsync(GetReportRequest request);
        Task<IEnumerable<NetworkActivationReportModel>> GetNetworkWiseActivationsAsync(GetReportRequest request);
        Task<IEnumerable<NetworkActivationReportModel>> GetSimAllocationReportAsync(GetReportRequest request);
        Task<IEnumerable<DashboardMetricsModel>> GetDahboardMetricsAsync(GetReportRequest request);
        Task<IEnumerable<DashboardChartMetricsModel>> GetDahboardChartActivationMetricsAsync(GetReportRequest request);

    }
}
