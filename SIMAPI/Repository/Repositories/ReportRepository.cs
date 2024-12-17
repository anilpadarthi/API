using Microsoft.Data.SqlClient;
using SIMAPI.Data;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Models.Report;
using SIMAPI.Data.Models.Report.InstantReport;
using SIMAPI.Repository.Interfaces;

namespace SIMAPI.Repository.Repositories
{
    public class ReportRepository : Repository, IReportRepository
    {
        public ReportRepository(SIMDBContext context) : base(context)
        {
        }

        public async Task<IEnumerable<MonthlyActivationModel>> GetMonthlyActivationsAsync(GetReportRequest request)
        {
            var sqlParameters = new[]
            {
                new SqlParameter("@filterMode", request.filterMode),
                 !string.IsNullOrEmpty( request.fromDate) ? new SqlParameter("@date", request.fromDate) : new SqlParameter("@date", DBNull.Value),
                 request.userId.HasValue ? new SqlParameter("@userId", request.userId) : new SqlParameter("@userId", DBNull.Value),
                 !string.IsNullOrEmpty( request.userRole) ? new SqlParameter("@userRole", request.userRole) : new SqlParameter("@userRole", DBNull.Value),
                 !string.IsNullOrEmpty( request.filterType) ? new SqlParameter("@filterType", request.filterType) : new SqlParameter("@filterType", DBNull.Value),
                 request.filterId.HasValue ? new SqlParameter("@filterId", request.filterId) : new SqlParameter("@filterId", DBNull.Value),
                new SqlParameter("@isInstantActivation", request.isInstantActivation ? 1: 0)
            };
            return await ExecuteStoredProcedureAsync<MonthlyActivationModel>("exec [dbo].[Monthly_Activations] @filterMode, @date, @userId, @userRole,@filterType, @filterId, @isInstantActivation", sqlParameters);
        }

        public async Task<List<dynamic>> GetMonthlyHistoryActivationsAsync(GetReportRequest request)
        {
            var sqlParameters = new[]
            {
                 new SqlParameter("@filterMode", request.filterMode),
                 !string.IsNullOrEmpty( request.fromDate) ? new SqlParameter("@fromDate", request.fromDate) : new SqlParameter("@fromDate", DBNull.Value),
                 !string.IsNullOrEmpty( request.toDate) ? new SqlParameter("@toDate", request.toDate) : new SqlParameter("@toDate", DBNull.Value),
                 request.userId.HasValue ? new SqlParameter("@userId", request.userId) : new SqlParameter("@userId", DBNull.Value),
                 !string.IsNullOrEmpty( request.userRole) ? new SqlParameter("@userRole", request.userRole) : new SqlParameter("@userRole", DBNull.Value),
                 !string.IsNullOrEmpty( request.filterType) ? new SqlParameter("@filterType", request.filterType) : new SqlParameter("@filterType", DBNull.Value),
                 request.filterId.HasValue ? new SqlParameter("@filterId", request.filterId) : new SqlParameter("@filterId", DBNull.Value),
                new SqlParameter("@isInstantActivation", request.isInstantActivation ? 1: 0)
            };
            //return await ExecuteStoredProcedureAsync<MonthlyHistoryActivationModel>("exec [dbo].[Monthly_History_Activations] @filterMode, @fromDate,@toDate, @userId, @userRole,@filterType,@filterId, @isInstantActivation", sqlParameters);
            return await GetDataTable("Monthly_History_Activations", sqlParameters);
        }

        public async Task<IEnumerable<DailyGivenCountModel>> GetDailyGivenCountAsync(GetReportRequest request)
        {
            var sqlParameters = new[]
            {
                 request.loggedInUserId.HasValue ? new SqlParameter("@userId", request.loggedInUserId) : new SqlParameter("@userId", DBNull.Value),
                 request.userRoleId.HasValue ? new SqlParameter("@userRoleId", request.userRoleId) : new SqlParameter("@userRoleId", DBNull.Value),
                 new SqlParameter("@filterType", request.filterType ?? ""),
                 new SqlParameter("@filterId", request.filterId ?? 0),
                 !string.IsNullOrEmpty( request.fromDate) ? new SqlParameter("@date", request.fromDate) : new SqlParameter("@date", DBNull.Value)
            };
            return await ExecuteStoredProcedureAsync<DailyGivenCountModel>("exec [dbo].[Get_Daily_Given_Count] @userId, @userRoleId, @filterType, @filterId, @date", sqlParameters);
        }

        public async Task<IEnumerable<NetworkUsageModel>> GetNetworkUsageReportAsync(GetReportRequest request)
        {
            var sqlParameters = new[]
            {
                 request.loggedInUserId.HasValue ? new SqlParameter("@userId", request.loggedInUserId) : new SqlParameter("@userId", DBNull.Value),
                 !string.IsNullOrEmpty( request.userRole) ? new SqlParameter("@userRole", request.userRole) : new SqlParameter("@userRole", DBNull.Value),
                 new SqlParameter("@filterType", request.filterType ?? ""),
                 new SqlParameter("@filterId", request.filterId ?? 0),
                 !string.IsNullOrEmpty( request.fromDate) ? new SqlParameter("@date", request.fromDate) : new SqlParameter("@date", DBNull.Value)
            };
            return await ExecuteStoredProcedureAsync<NetworkUsageModel>("exec [dbo].[Get_Network_Usage_Count] @userId, @userRole, @filterType, @filterId, @date", sqlParameters);
        }

        public async Task<IEnumerable<KPITargetReportModel>> GetKPITargetReportAsync(GetReportRequest request)
        {
            var sqlParameters = new[]
             {
                 request.userId.HasValue ? new SqlParameter("@loggedInUserId", request.loggedInUserId) : new SqlParameter("@loggedInUserId", DBNull.Value),
                  new SqlParameter("@loggedInUserRoleId", request.userRoleId ?? 0),
                 new SqlParameter("@filterUserRoleId", request.filterUserRoleId ?? 0),
                 new SqlParameter("@filterId", request.filterId ?? 0),
                 !string.IsNullOrEmpty( request.fromDate) ? new SqlParameter("@date", request.fromDate) : new SqlParameter("@date", DBNull.Value)
            };
            return await ExecuteStoredProcedureAsync<KPITargetReportModel>("exec [dbo].[Monthly_KPI_Targets] @loggedInUserId, @loggedInUserRoleId, @filterUserRoleId, @filterId, @date", sqlParameters);
        }

        public async Task<IEnumerable<MonthlyUserActivationModel>> GetMonthlyUserActivationsAsync(GetReportRequest request)
        {
            var sqlParameters = new[]
            {
                new SqlParameter("@date", request.fromDate),
                new SqlParameter("@userId", request.userId),
                new SqlParameter("@userRole", request.userRoleId),
                new SqlParameter("@filterType", request.filterType),
                new SqlParameter("@filterId", request.filterId),
                new SqlParameter("@activationType", request.activationType)
            };
            return await ExecuteStoredProcedureAsync<MonthlyUserActivationModel>("exec [dbo].[Monthly_User_Activations] @date, @userId, @userRole,@filterType,@filterId, @activationType", sqlParameters);
        }

        public async Task<IEnumerable<MonthlyAreaActivationModel>> GetMonthlyAreaActivationsAsync(GetReportRequest request)
        {
            var sqlParameters = new[]
            {
                new SqlParameter("@date", request.fromDate),
                new SqlParameter("@userId", request.userId),
                new SqlParameter("@userRole", request.userRole),
                new SqlParameter("@filterType", request.filterType ?? ""),
                new SqlParameter("@filterId", request.filterId ?? 0),
                new SqlParameter("@activationType", request.activationType ??  "")
            };
            return await ExecuteStoredProcedureAsync<MonthlyAreaActivationModel>("exec [dbo].[Monthly_Area_Activations] @date, @userId, @userRole,@filterType,@filterId, @activationType", sqlParameters);
        }

        public async Task<IEnumerable<MonthlyShopActivationModel>> GetMonthlyShopActivationsAsync(GetReportRequest request)
        {
            var sqlParameters = new[]
            {
                new SqlParameter("@date", request.fromDate),
                new SqlParameter("@userId", request.userId),
                new SqlParameter("@userRole", request.userRole),
                new SqlParameter("@filterType", request.filterType ?? ""),
                new SqlParameter("@filterId", request.filterId ?? 0),
                new SqlParameter("@activationType", request.activationType ??  "")
            };
            return await ExecuteStoredProcedureAsync<MonthlyShopActivationModel>("exec [dbo].[Monthly_Shop_Activations] @date, @userId, @userRole,@filterType,@filterId, @activationType", sqlParameters);
        }



        public async Task<IEnumerable<InstantActivationReportModel>> GetInstantActivationReportAsync(GetReportRequest request)
        {
            var sqlParameters = new[]
            {
                new SqlParameter("@userId", request.userId),
                new SqlParameter("@userRoleId", request.userRoleId),
                new SqlParameter("@filterType", request.filterType),
                new SqlParameter("@filterId", request.filterId),
                new SqlParameter("@date", request.fromDate),
                new SqlParameter("@reportType", request.reportType)
            };
            return await ExecuteStoredProcedureAsync<InstantActivationReportModel>("exec [dbo].[Get_Instant_Activations] @reportType, @userId, @userRoleId,@filterType,@filterId,@date", sqlParameters);
        }








        public async Task<IEnumerable<SalaryReportModel>> GetSalaryReportAsync(GetReportRequest request)
        {
            var sqlParameters = new[]
            {
                new SqlParameter("@userId", request.userId),
                new SqlParameter("@userRoleId", request.userRoleId),
                new SqlParameter("@filterType", request.filterType),
                new SqlParameter("@filterId", request.filterId),
                new SqlParameter("@date", request.fromDate)
            };
            return await ExecuteStoredProcedureAsync<SalaryReportModel>("exec [dbo].[Get_Last_Daily_Activations] @userId, @userRoleId,@filterType,@filterId,@date", sqlParameters);
        }
    }
}
