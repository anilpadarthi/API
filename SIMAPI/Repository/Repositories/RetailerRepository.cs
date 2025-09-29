using Microsoft.Data.SqlClient;
using SIMAPI.Data;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Models.Retailer;
using SIMAPI.Repository.Interfaces;

namespace SIMAPI.Repository.Repositories
{
    public class RetailerRepository : Repository, IRetailerRepository
    {
        public RetailerRepository(SIMDBContext context) : base(context)
        {
        }


        public async Task<IEnumerable<ActivationModel>> GetActvationsAsync(GetReportRequest request)
        {
            var sqlParameters = new[]
            {
                 request.loggedInUserId.HasValue ? new SqlParameter("@userId", request.loggedInUserId) : new SqlParameter("@userId", DBNull.Value),
                 !string.IsNullOrEmpty( request.userRole) ? new SqlParameter("@userRole", request.userRole) : new SqlParameter("@userRole", DBNull.Value),
                 new SqlParameter("@filterType", request.filterType ?? ""),
                 new SqlParameter("@filterId", request.filterId ?? 0),
                 !string.IsNullOrEmpty( request.fromDate) ? new SqlParameter("@date", request.fromDate) : new SqlParameter("@date", DBNull.Value)
            };
            return await ExecuteStoredProcedureAsync<ActivationModel>("exec [dbo].[Get_Network_Usage_Count] @userId, @userRole, @filterType, @filterId, @date", sqlParameters);
        }

        public async Task<IEnumerable<ShopCommissionModel>> GetCommissionsAsync(GetReportRequest request)
        {
            var sqlParameters = new[]
            {
                 request.loggedInUserId.HasValue ? new SqlParameter("@userId", request.loggedInUserId) : new SqlParameter("@userId", DBNull.Value),
                 !string.IsNullOrEmpty( request.userRole) ? new SqlParameter("@userRole", request.userRole) : new SqlParameter("@userRole", DBNull.Value),
                 new SqlParameter("@filterType", request.filterType ?? ""),
                 new SqlParameter("@filterId", request.filterId ?? 0),
                 !string.IsNullOrEmpty( request.fromDate) ? new SqlParameter("@date", request.fromDate) : new SqlParameter("@date", DBNull.Value)
            };
            return await ExecuteStoredProcedureAsync<ShopCommissionModel>("exec [dbo].[Get_Network_Usage_Count] @userId, @userRole, @filterType, @filterId, @date", sqlParameters);
        }

        public async Task<IEnumerable<StockVsConnectionModel>> GetStockVsConnectionsAsync(GetReportRequest request)
        {
            var sqlParameters = new[]
            {
                 request.loggedInUserId.HasValue ? new SqlParameter("@userId", request.loggedInUserId) : new SqlParameter("@userId", DBNull.Value),
                 !string.IsNullOrEmpty( request.userRole) ? new SqlParameter("@userRole", request.userRole) : new SqlParameter("@userRole", DBNull.Value),
                 new SqlParameter("@filterType", request.filterType ?? ""),
                 new SqlParameter("@filterId", request.filterId ?? 0),
                 !string.IsNullOrEmpty( request.fromDate) ? new SqlParameter("@date", request.fromDate) : new SqlParameter("@date", DBNull.Value)
            };
            return await ExecuteStoredProcedureAsync<StockVsConnectionModel>("exec [dbo].[Get_Network_Usage_Count] @userId, @userRole, @filterType, @filterId, @date", sqlParameters);
        }
    }
}
