using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SIMAPI.Data;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models.CommissionStatement;
using SIMAPI.Repository.Interfaces;

namespace SIMAPI.Repository.Repositories
{
    public class CommissionStatementRepository : Repository, ICommissionStatementRepository
    {
        public CommissionStatementRepository(SIMDBContext context) : base(context)
        {
        }


        public async Task<ShopCommissionHistory?> GetCommissionHistoryDetailsAsync(int shopCommissionHistoryId)
        {
            return await _context.Set<ShopCommissionHistory>()
                .Where(w => w.ShopCommissionHistoryId == shopCommissionHistoryId)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ShopCommissionHistory>> GetCommissionHistoryListAsync(string referenceNumber)
        {
            return await _context.Set<ShopCommissionHistory>()
                .Where(w => w.ReferenceNumber == referenceNumber)
                .ToListAsync();
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
            return await ExecuteStoredProcedureAsync<CommissionStatementModel>("exec [dbo].[Get_Commission_Statement] @shopId,@date", sqlParameters);
        }

        public async Task<IEnumerable<CommissionShopListModel>> GetCommissionShopList(GetReportRequest request)
        {
            var sqlParameters = new[]
            {
                request.areaId!=null ? new SqlParameter("@areaId", request.areaId) : new SqlParameter("@areaId", DBNull.Value),
                request.shopId!=null ? new SqlParameter("@shopId", request.shopId) : new SqlParameter("@shopId", DBNull.Value),
                new SqlParameter("@date", request.fromDate),
                new SqlParameter("@isOptedForCheques", request.isOptedForCheques)
            };
            return await ExecuteStoredProcedureAsync<CommissionShopListModel>("exec [dbo].[Get_Commission_Statement_Shop_List] @areaId,@shopId,@date,@isOptedForCheques", sqlParameters);
        }


    }
}
