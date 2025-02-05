using Microsoft.Data.SqlClient;
using SIMAPI.Data;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Models.Dashboard;
using SIMAPI.Repository.Interfaces;

namespace SIMAPI.Repository.Repositories
{
    public class DashboardRepository : Repository, IDashboardRepository
    {
        public DashboardRepository(SIMDBContext context) : base(context)
        {
        }



        public async Task<IEnumerable<NetworkActivationReportModel>> GetSimAllocationReportAsync(GetReportRequest request)
        {
            var sqlParameters = new[]
            {
               new SqlParameter("@shopId", request.shopId),
                new SqlParameter("@userId", request.userId),
                new SqlParameter("@fromDate",request.fromDate),
                new SqlParameter("@toDate", request.toDate)
            };
            return await ExecuteStoredProcedureAsync<NetworkActivationReportModel>("exec [dbo].[OnField_Commission] @shopId, @userId, @fromDate, @toDate", sqlParameters);
        }

        public async Task<IEnumerable<UserWiseKPIReportModel>> GetUserWiseKPIReportAsync(GetReportRequest request)
        {
            var sqlParameters = new[]
           {
                new SqlParameter("@date", request.fromDate),
                new SqlParameter("@userRole", request.filterType),
                new SqlParameter("@filterId", request.filterId)
            };
            return await ExecuteStoredProcedureAsync<UserWiseKPIReportModel>("exec [dbo].[Dashboard_Agent_KPI_Details] @date,@userRole,@filterId", sqlParameters);
        }

        public async Task<IEnumerable<NetworkActivationReportModel>> GetNetworkWiseActivationsAsync(GetReportRequest request)
        {
            var sqlParameters = new[]
            {
                new SqlParameter("@date", request.fromDate),
                new SqlParameter("@filterType", request.filterType ?? "" ),
                new SqlParameter("@filterId", request.filterId ?? 0)
            };
            return await ExecuteStoredProcedureAsync<NetworkActivationReportModel>("exec [dbo].[Dashboard_Network_Wise_Activations] @date,@filterType,@filterId", sqlParameters);
        }

        public async Task<IEnumerable<UserWiseActivationReportModel>> GetUserWiseActivationsAsync(GetReportRequest request)
        {
            var sqlParameters = new[]
              {
                new SqlParameter("@date", request.fromDate),
                new SqlParameter("@filterType", request.filterType ?? "" ),
                new SqlParameter("@filterId", request.filterId ?? 0)
            };
            return await ExecuteStoredProcedureAsync<UserWiseActivationReportModel>("exec [dbo].[Dashboard_Agent_Wise_Activations] @date,@filterType,@filterId", sqlParameters);
        }

        public async Task<IEnumerable<AreaWiseActivationReportModel>> GetAreaWiseActivationsAsync(GetReportRequest request)
        {
            var sqlParameters = new[]
              {
                new SqlParameter("@date", request.fromDate),
                new SqlParameter("@filterType", request.filterType ?? "" ),
                new SqlParameter("@filterId", request.filterId ?? 0)
            };
            return await ExecuteStoredProcedureAsync<AreaWiseActivationReportModel>("exec [dbo].[Dashboard_Area_Wise_Activations] @date,@filterType,@filterId", sqlParameters);
        }

        public async Task<IEnumerable<DashboardMetricsModel>> GetDahboardMetricsAsync(GetReportRequest request)
        {
            var sqlParameters = new[]
            {
                new SqlParameter("@date", request.fromDate),
                new SqlParameter("@filterType", request.filterType),
                new SqlParameter("@filterId", request.filterId)
            };
            return await ExecuteStoredProcedureAsync<DashboardMetricsModel>("exec [dbo].[Dashboard_Metrics] @date,@filterType,@filterId", sqlParameters);
        }

        public async Task<IEnumerable<DashboardChartMetricsModel>> GetDahboardChartActivationMetricsAsync(GetReportRequest request)
        {
            var sqlParameters = new[]
            {
                new SqlParameter("@fromDate",request.fromDate),
                new SqlParameter("@toDate", request.toDate),
                new SqlParameter("@userId", request.userId),
                new SqlParameter("@userRole", request.userRole),
                new SqlParameter("@filterType", request.filterType),
                new SqlParameter("@filterId", request.filterId),
                new SqlParameter("@actvationType", request.activationType),
            };
            return await ExecuteStoredProcedureAsync<DashboardChartMetricsModel>("exec [dbo].[Dahboard_Chart_Activation_Metrics] @fromDate, @toDate,@userId,@userRole,@filterType,@filterId,@actvationType", sqlParameters);
        }

        
    }
}
