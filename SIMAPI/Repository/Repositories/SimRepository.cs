using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SIMAPI.Data;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models.CommissionStatement;
using SIMAPI.Data.Models.Sim;
using SIMAPI.Data.Models.Tracking;
using SIMAPI.Repository.Interfaces;
using System.Text;

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

        public async Task<SimMap?> GetSimMapDetailsAsync(int SimId)
        {
            return await _context.Set<SimMap>()
               .Where(w => w.SimId == SimId)
               .SingleOrDefaultAsync();
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
