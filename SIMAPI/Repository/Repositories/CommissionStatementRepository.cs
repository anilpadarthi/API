using Microsoft.Data.SqlClient;
using SIMAPI.Data;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Models.CommissionStatement;
using SIMAPI.Repository.Interfaces;

namespace SIMAPI.Repository.Repositories
{
    public class CommissionStatementRepository : Repository, ICommissionStatementRepository
    {
        public CommissionStatementRepository(SIMDBContext context) : base(context)
        {
        }

        public async Task<IEnumerable<CommissionListModel>> GetCommissionListAsync(GetReportRequest request)
        {
            var sqlParameters = new[]
             {
                new SqlParameter("@fromDate", request.fromDate ?? ""),
                new SqlParameter("@toDate", request.toDate ?? ""),
                new SqlParameter("@areaId", request.areaId ?? 0),
                new SqlParameter("@shopId", request.shopId ?? 0),
                new SqlParameter("@userId", request.userId ?? 0),

            };
            return await ExecuteStoredProcedureAsync<CommissionListModel>("exec [dbo].[Get_Commission_List] @fromDate,@toDate, @areaId,@shopId,@userId", sqlParameters);
        }

        public async Task<IEnumerable<CommissionStatementModel?>> GetCommissionStatementAsync(GetReportRequest request)
        {
            var sqlParameters = new[]
            {
                new SqlParameter("@shopId", request.shopId),
                new SqlParameter("@date", request.fromDate)
            };
            return await ExecuteStoredProcedureAsync<CommissionStatementModel>("exec [dbo].[Get_CommissionStatement] @shopId,@date", sqlParameters);
        }

        public async Task<IEnumerable<CommissionShopListModel>> GetShopListForCommission(GetReportRequest request)
        {
            var sqlParameters = new[]
            {
                new SqlParameter("@areaId", request.areaId),
                new SqlParameter("@shopId", request.shopId),
                new SqlParameter("@fromDate", request.fromDate),
                new SqlParameter("@toDate", request.toDate)
            };
            return await ExecuteStoredProcedureAsync<CommissionShopListModel>("exec [dbo].[Get_Shop_List_For_Commission_Statement] @areaId,@shopId,@fromDate,@toDate", sqlParameters);
        }


    }
}
