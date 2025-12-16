using Microsoft.Data.SqlClient;
using SIMAPI.Data;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models.OrderListModels;
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
                new SqlParameter("@isInstantActivation", request.isInstantActivation.Value ? 1: 0)
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
                new SqlParameter("@isInstantActivation", request.isInstantActivation.Value ? 1: 0)
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

        public async Task<SalaryReportModel> GetSalaryReportAsync(GetReportRequest request)
        {
            SalaryReportModel salaryReportModel = new SalaryReportModel();
            var sqlParameters = new[]
            {
                new SqlParameter("@filterType", request.filterType),
                new SqlParameter("@filterId", request.filterId),
                new SqlParameter("@date", request.fromDate)
            };
            salaryReportModel.salaryDetailsModel = await ExecuteStoredProcedureAsync<SalaryDetailsModel>("exec [dbo].[Get_Salary_Details] @filterType,@filterId,@date", sqlParameters);
            salaryReportModel.salarySimCommissionDetailsModel = await ExecuteStoredProcedureAsync<SalarySimCommissionDetailsModel>("exec [dbo].[Get_Salary_Sim_Commission_Details] @filterType,@filterId,@date", sqlParameters);
            salaryReportModel.salaryAccessoriesCommissionDetailsModel = await ExecuteStoredProcedureAsync<SalaryAccessoriesCommissionDetailsModel>("exec [dbo].[Get_Salary_Accessories_Commission_Details] @filterType,@filterId,@date", sqlParameters);
            salaryReportModel.salaryTransactions = await ExecuteStoredProcedureAsync<UserSalaryTransaction>("exec [dbo].[Get_Salary_Transactions] @filterType,@filterId,@date", sqlParameters);
            return salaryReportModel;
        }

        public async Task<IEnumerable<SimAllocationModel>> GetSimAllocationReportAsync(GetReportRequest request)
        {
            var sqlParameters = new[]
             {
                 request.loggedInUserId.HasValue ? new SqlParameter("@loggedInUserId", request.loggedInUserId) : new SqlParameter("@loggedInUserId", DBNull.Value),
                  new SqlParameter("@loggedInUserRoleId", request.userRoleId ?? 0),
                 new SqlParameter("@filterUserRoleId", request.filterUserRoleId ?? 0),
                 new SqlParameter("@filterId", request.filterId ?? 0),
                 !string.IsNullOrEmpty( request.fromDate) ? new SqlParameter("@date", request.fromDate) : new SqlParameter("@date", DBNull.Value)
            };
            return await ExecuteStoredProcedureAsync<SimAllocationModel>("exec [dbo].[Monthly_Sim_Allocations] @loggedInUserId, @loggedInUserRoleId, @filterUserRoleId, @filterId, @date", sqlParameters);
        }

        public async Task<OutstandingAmountModel?> GetAccessoriesOutstandingReportsAsync(GetReportRequest request)
        {

            var sqlParameters = new[]
            {
                new SqlParameter("@filterType", request.filterType),
                new SqlParameter("@filterId", request.filterId),
            };
            return (await ExecuteStoredProcedureAsync<OutstandingAmountModel>("exec [dbo].[Get_Accessories_Outstanding_Amounts] @filterType,@filterId", sqlParameters)).FirstOrDefault();
        }

        public async Task<IEnumerable<MonthlyAccessoriesReportModel>> GetMonthlyAccessoriesReportAsync(GetReportRequest request)
        {

            var sqlParameters = new[]
            {
                new SqlParameter("@filterType", request.filterType??""),
                new SqlParameter("@filterId", request.filterId??0),
                new SqlParameter("@date", request.fromDate)
            };
            return await ExecuteStoredProcedureAsync<MonthlyAccessoriesReportModel>("exec [dbo].[Get_Monthly_Accessories_Report] @date,@filterId,@filterType", sqlParameters);
        }

        public async Task<IEnumerable<MonthlyAccessoriesCommissionPercentReportModel>> GetMonthlyAccessoriesCommissionPercentReportAsync(GetReportRequest request)
        {

            var sqlParameters = new[]
            {
                new SqlParameter("@filterType", request.filterType??""),
                new SqlParameter("@filterId", request.filterId??0),
                new SqlParameter("@date", request.fromDate)
            };
            return await ExecuteStoredProcedureAsync<MonthlyAccessoriesCommissionPercentReportModel>("exec [dbo].[Get_Monthly_Accessories_Commission_Percent_Report] @date,@filterId,@filterType", sqlParameters);
        }

        public async Task<IEnumerable<AccessoriesReportDetailModel>> GetDetailsAccessoriesReportAsync(GetReportRequest request)
        {
            var sqlParameters = new[]
            {
                new SqlParameter("@filterType", request.filterType ?? ""),
                new SqlParameter("@filterId", request.filterId ?? 0),
                new SqlParameter("@date", request.fromDate)
            };
            return await ExecuteStoredProcedureAsync<AccessoriesReportDetailModel>("exec [dbo].[Get_Monthly_Accessories_Report] @date,@filterId,@filterType", sqlParameters);
        }

        public async Task<IEnumerable<GetChequeWithdrawnReportModel>> GetChequeWithdrawnReportsAsync(GetReportRequest request)
        {
            var sqlParameters = new[]
           {
                new SqlParameter("@fromDate", request.fromDate),
                new SqlParameter("@toDate", request.toDate)
            };
            return await ExecuteStoredProcedureAsync<GetChequeWithdrawnReportModel>("exec [dbo].[GetChequeWithdrawnReports] @fromDate,@toDate", sqlParameters);

        }

        public async Task<IEnumerable<BankChequeStatusModel>> GetBankChequeStatusAsync(string chequeNumber)
        {
            var sqlParameters = new[]
           {
                new SqlParameter("@CheuqeNo", chequeNumber),
            };
            return await ExecuteStoredProcedureAsync<BankChequeStatusModel>("exec [dbo].[ChequeSearch] @CheuqeNo", sqlParameters);

        }

        public async Task<IEnumerable<DownloadDailyActivationModel>> DownloadDailyActivtionsAsync(GetReportRequest request)
        {
            var sqlParameters = new[]
            {
                new SqlParameter("@date", request.fromDate),
                new SqlParameter("@filterType", request.userRole?? ""),
                new SqlParameter("@filterId", request.userId?? 0)
            };
            return await ExecuteStoredProcedureAsync<DownloadDailyActivationModel>("exec [dbo].[Download_MonthlyConnections] @date,@filterType,@filterId", sqlParameters);
        }

        public async Task<List<dynamic>> DownloadActivtionAnalysisReportAsync(GetReportRequest request)
        {
            var sqlParameters = new[]
            {
                new SqlParameter("@fromMonth", Convert.ToDateTime(request.fromDate).Month),
            new SqlParameter("@fromYear", Convert.ToDateTime(request.fromDate).Year),
            new SqlParameter("@toMonth", Convert.ToDateTime(request.toDate).Month),
            new SqlParameter("@toYear", Convert.ToDateTime(request.toDate).Year),
            request.userId.HasValue ? new SqlParameter("@userId", request.loggedInUserId) : new SqlParameter("@userId", DBNull.Value),
            !string.IsNullOrEmpty(request.filterType) ? new SqlParameter("@filterType", request.filterType) : new SqlParameter("@filterType", DBNull.Value),
            request.filterId.HasValue ? new SqlParameter("@filterId", request.filterId) : new SqlParameter("@filterId", DBNull.Value),
        };
            //return await ExecuteStoredProcedureAsync<MonthlyHistoryActivationModel>("exec [dbo].[Monthly_History_Activations] @filterMode, @fromDate,@toDate, @userId, @userRole,@filterType,@filterId, @isInstantActivation", sqlParameters);
            return await GetDataTable("Download_All_Shop_History_Activations", sqlParameters);
        }
        
    }
}
