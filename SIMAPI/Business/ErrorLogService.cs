using Microsoft.EntityFrameworkCore;
using SIMAPI.Data;
using SIMAPI.Data.Entities;

namespace SIMAPI.Business
{
    public class ErrorLogService
    {
        private readonly IDbContextFactory<SIMDBContext> _factory;

        public ErrorLogService(IDbContextFactory<SIMDBContext> factory)
        {
            _factory = factory;
        }

        public async Task LogErrorAsync(Exception ex, string optional = "")
        {
            try
            {
                await using var db = _factory.CreateDbContext();
                var errorLog = new ErrorInfo
                {
                    ErrorMessage = ex.Message + optional,
                    StackTrace = ex.StackTrace,
                    Method = ex.Source,
                    CreatedDate = DateTime.Now
                };

                db.Add(errorLog);
                await db.SaveChangesAsync();
            }
            catch { }
        }
    }

}
