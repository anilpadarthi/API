using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SIMAPI.Data;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models.Sim;
using SIMAPI.Repository.Interfaces;
using System.Text;
using System.Data;

namespace SIMAPI.Repository.Repositories
{
    public class SimRepository : Repository, ISimRepository
    {
        public SimRepository(SIMDBContext context) : base(context)
        {
        }

        public async Task<bool> IsValidSim(string IMEI)
        {
            return await _context.Set<Sim>()
               .AnyAsync(w => w.IMEI == IMEI);
        }

        public async Task<Sim?> GetSimDetailsAsync(string IMEI)
        {
            var query = _context.Set<Sim>()
               .Where(w => w.IMEI == IMEI);
            return await query.SingleOrDefaultAsync();
        }

        public async Task DeAllocateFromSyncSimAPI(int SimId)
        {
            var details = await _context.Set<SimAPI>()
               .Where(w => w.SimId == SimId)
               .SingleOrDefaultAsync();
            if (details != null)
            {
                _context.Remove(details);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<SimMap?> GetSimMapDetailsAsync(int SimId)
        {
            return await _context.Set<SimMap>()
               .Where(w => w.SimId == SimId)
               .SingleOrDefaultAsync();
        }

        public async Task<object?> AllocateSimsAsync(int shopId, int loggedInUserId, DataTable dt)
        {
            var sqlParameters = new[]
            {
                new SqlParameter("@ShopId", shopId),
                new SqlParameter("@LoggedInUserId", loggedInUserId),
                new SqlParameter("@ImeiNumbers", dt)
            };
            var result = await GetScalar("AllocateSims", sqlParameters);
            return result;
        }

        public async Task<object?> DeAllocateSimsAsync(int shopId, int loggedInUserId, DataTable dt)
        {
            var sqlParameters = new[]
            {
                new SqlParameter("@ShopId", shopId),
                new SqlParameter("@LoggedInUserId", loggedInUserId),
                new SqlParameter("@ImeiNumbers", dt)
            };
            var result = await GetScalar("DeAllocateSims", sqlParameters);
            return result;
        }

        public async Task<IEnumerable<SimHistoryModel>> GetSimHistoryDetailsAsync(StringBuilder simNumbersBuilder)
        {
            var sqlParameters = new[]
            {
                new SqlParameter("@simNumbers", simNumbersBuilder.ToString())
            };
            return await ExecuteStoredProcedureAsync<SimHistoryModel>("exec [dbo].[Get_Sim_History_Details] @simNumbers", sqlParameters);
        }

        public async Task<IEnumerable<SimScanModel>> ScanSimsAsync(StringBuilder simNumbersBuilder)
        {

            var sqlParameters = new[]
            {
                new SqlParameter("@simNumbers", simNumbersBuilder.ToString())
            };
            return await ExecuteStoredProcedureAsync<SimScanModel>("exec [dbo].[Scan_Sims] @simNumbers", sqlParameters);
        }
    }
}
