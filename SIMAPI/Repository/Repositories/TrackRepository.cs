using Microsoft.Data.SqlClient;
using SIMAPI.Data;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Models.Tracking;
using SIMAPI.Repository.Interfaces;

namespace SIMAPI.Repository.Repositories
{
    public class TrackRepository : Repository, ITrackRepository
    {
        public TrackRepository(SIMDBContext context) : base(context)
        {
        }

        public async Task<IEnumerable<TrackReportModel>> GetTrackReportAsync(GetReportRequest request)
        {
            var sqlParameters = new[]
            {
                new SqlParameter("@userId", request.userId),
                new SqlParameter("@userRoleId", request.userRoleId),
                new SqlParameter("@filterType", request.filterType ?? ""),
                new SqlParameter("@filterId", request.filterId ?? 0),
                new SqlParameter("@date", request.fromDate)
            };
            return await ExecuteStoredProcedureAsync<TrackReportModel>("exec [dbo].[Get_Tracking_Report] @userId, @userRoleId,@filterType,@filterId,@date", sqlParameters);
        }

        public async Task<IEnumerable<AreaVisitedModel>> GetAreasVisitedReportAsync(GetReportRequest request)
        {
            var sqlParameters = new[]
            {
                 new SqlParameter("@userId", request.userId),
                new SqlParameter("@userRoleId", request.userRoleId),
                new SqlParameter("@filterType", request.filterType ?? ""),
                new SqlParameter("@filterId", request.filterId ?? 0),
                new SqlParameter("@date", request.fromDate)
            };
            return await ExecuteStoredProcedureAsync<AreaVisitedModel>("exec [dbo].[Get_Tracking_Report] @userId, @userRoleId,@filterType,@filterId,@date", sqlParameters);
        }

        public async Task<IEnumerable<DailyReportModel>> GetDailyGivenReportAsync(GetReportRequest request)
        {
            var sqlParameters = new[]
            {
                new SqlParameter("@userId", request.userId),
                new SqlParameter("@userRoleId", request.userRoleId),
                new SqlParameter("@filterType", request.filterType),
                new SqlParameter("@filterId", request.filterId),
                new SqlParameter("@date", request.fromDate)
            };
            return await ExecuteStoredProcedureAsync<DailyReportModel>("exec [dbo].[Get_Daily_Reports] @userId, @userRoleId,@filterType,@filterId,@date", sqlParameters);
        }

        public async Task<List<dynamic>> GetShopsSimsGivenReportAsync(GetReportRequest request)
        {
            var sqlParameters = new[]
            {
                 new SqlParameter("@userId", request.userId),
                new SqlParameter("@userRoleId", request.userRoleId),
                new SqlParameter("@filterType", request.filterType ?? ""),
                new SqlParameter("@filterId", request.filterId ?? 0),
                new SqlParameter("@date", request.fromDate)
            };
            //return await ExecuteStoredProcedureAsync<ShopVisitedModel>("exec [dbo].[Get_Tracking_Report] @userId, @userRoleId,@filterType,@filterId,@date", sqlParameters);
            return await GetDataTable("Get_Tracking_Report", sqlParameters);
        }

        public async Task<IEnumerable<ShopVisitedModel>> GetShopsVisitedReportAsync(GetReportRequest request)
        {
            var sqlParameters = new[]
             {
                 new SqlParameter("@userId", request.userId),
                new SqlParameter("@userRoleId", request.userRoleId),
                new SqlParameter("@filterType", request.filterType ?? ""),
                new SqlParameter("@filterId", request.filterId ?? 0),
                new SqlParameter("@date", request.fromDate)
            };
            return await ExecuteStoredProcedureAsync<ShopVisitedModel>("exec [dbo].[Get_Tracking_Report] @userId, @userRoleId,@filterType,@filterId,@date", sqlParameters);
        }



        public async Task<IEnumerable<UserTrackDataModel>> GetUserTrackDataReportAsync(GetReportRequest request)
        {
            var sqlParameters = new[]
            {
                 new SqlParameter("@userId", request.userId),
                new SqlParameter("@userRoleId", request.userRoleId),
                new SqlParameter("@filterType", request.filterType ?? ""),
                new SqlParameter("@filterId", request.filterId ?? 0),
                new SqlParameter("@date", request.fromDate)
            };
            return await ExecuteStoredProcedureAsync<UserTrackDataModel>("exec [dbo].[Get_Tracking_Report] @userId, @userRoleId,@filterType,@filterId,@date", sqlParameters);
        }

        public async Task<IEnumerable<LatLongInfoModel>> GetLatLongReportAsync(GetReportRequest request)
        {
            var sqlParameters = new[]
            {
                new SqlParameter("@userId", request.userId),
                new SqlParameter("@userRoleId", request.userRoleId),
                new SqlParameter("@filterType", request.filterType),
                new SqlParameter("@filterId", request.filterId),
                new SqlParameter("@date", request.fromDate)
            };
            return await ExecuteStoredProcedureAsync<LatLongInfoModel>("exec [dbo].[Get_Tracking_Report] @userId, @userRoleId,@filterType,@filterId,@date", sqlParameters);
        }

        public async Task<IEnumerable<LatLongInfoModel>> DownloadAttendaceAsync(string date)
        {
            var sqlParameters = new[]
            {
                new SqlParameter("@date", date),
            };
            return await ExecuteStoredProcedureAsync<LatLongInfoModel>("exec [dbo].[Download_Attendace] @date", sqlParameters);
        }
    }
}
