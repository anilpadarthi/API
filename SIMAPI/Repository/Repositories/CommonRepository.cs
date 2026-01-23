using SIMAPI.Data;
using SIMAPI.Data.Entities;
using SIMAPI.Repository.Interfaces;
using System.Net;
using System.Text.Json;

namespace SIMAPI.Repository.Repositories
{
    public class CommonRepository : Repository, ICommonRepository
    {

        public CommonRepository(SIMDBContext context) : base(context)
        {
        }

        public async Task LogError(Exception ex, string optional = "")
        {
            var errorLog = new ErrorInfo
            {
                ErrorMessage = ex.Message + optional,
                StackTrace = ex.StackTrace,
                Method = ex.Source,
                CreatedDate = DateTime.Now
            };

            _context.Add(errorLog);
            await _context.SaveChangesAsync();
        }
    }
}
