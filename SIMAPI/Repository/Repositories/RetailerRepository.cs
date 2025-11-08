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
                 new SqlParameter("@filterType",  "Monthly"),
                 new SqlParameter("@filterId", request.filterId ?? 0),
                 !string.IsNullOrEmpty( request.fromDate) ? new SqlParameter("@date", request.fromDate) : new SqlParameter("@date", DBNull.Value)
            };
            return await ExecuteStoredProcedureAsync<ActivationModel>("exec [dbo].[Get_Retailer_Activation_Report] @filterType, @filterId, @date", sqlParameters);
        }

        public async Task<IEnumerable<ActivationDetaiListModel>> GetActivationDetaiListAsync(GetReportRequest request)
        {
            var sqlParameters = new[]
            {
                 new SqlParameter("@filterType",  "Detail"),
                 new SqlParameter("@filterId", request.filterId ?? 0),
                 !string.IsNullOrEmpty( request.fromDate) ? new SqlParameter("@date", request.fromDate) : new SqlParameter("@date", DBNull.Value)
            };
            return await ExecuteStoredProcedureAsync<ActivationDetaiListModel>("exec [dbo].[Get_Retailer_Activation_Report] @filterType, @filterId, @date", sqlParameters);
        }
        public async Task<IEnumerable<SimGivenDetailListModel>> GetSimGivenAsync(GetReportRequest request)
        {
            var sqlParameters = new[]
            {
                 new SqlParameter("@filterType",  "Monthly"),
                 new SqlParameter("@filterId", request.filterId ?? 0),
                 !string.IsNullOrEmpty( request.fromDate) ? new SqlParameter("@date", request.fromDate) : new SqlParameter("@date", DBNull.Value)
            };
            return await ExecuteStoredProcedureAsync<SimGivenDetailListModel>("exec [dbo].[Get_Retailer_Sim_Given_Report] @filterType, @filterId, @date", sqlParameters);
        }


        public async Task<IEnumerable<RetailerCommissionListModel>> GetRetailerCommissionListAsync(GetReportRequest request)
        {
            var sqlParameters = new[]
            {
                 new SqlParameter("@filterType", request.filterType ?? ""),
                 new SqlParameter("@filterId", request.filterId ?? 0),
                 !string.IsNullOrEmpty( request.fromDate) ? new SqlParameter("@date", request.fromDate) : new SqlParameter("@date", DBNull.Value)
            };
            return await ExecuteStoredProcedureAsync<RetailerCommissionListModel>("exec [dbo].[Get_Retailer_Commission_Statement_List] @filterType, @filterId, @date", sqlParameters);
        }

        public async Task<IEnumerable<StockVsConnectionModel>> GetStockVsConnectionsAsync(GetReportRequest request)
        {
            var sqlParameters = new[]
            {
                 new SqlParameter("@filterType", request.filterType ?? ""),
                 new SqlParameter("@filterId", request.filterId ?? 0),
                 !string.IsNullOrEmpty( request.fromDate) ? new SqlParameter("@date", request.fromDate) : new SqlParameter("@date", DBNull.Value)
            };
            return await ExecuteStoredProcedureAsync<StockVsConnectionModel>("exec [dbo].[Get_Network_Usage_Count] @filterType, @filterId, @date", sqlParameters);
        }
    }
}
