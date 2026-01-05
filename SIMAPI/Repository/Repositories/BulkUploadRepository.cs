using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SIMAPI.Business.Enums;
using SIMAPI.Data;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models;
using SIMAPI.Repository.Interfaces;

namespace SIMAPI.Repository.Repositories
{
    public class BulkUploadRepository : Repository, IBulkUploadRepository
    {
        public BulkUploadRepository(SIMDBContext context) : base(context)
        {
        }

        

        public async Task<BulkUploadFile> GetAreaByIdAsync(int id)
        {
            return await _context.Set<BulkUploadFile>()
                .Where(w => w.BulkUploadFileId == id)
                .FirstOrDefaultAsync();
        }

        public async Task<List<dynamic>> DownloadTargetDataAsync(GetReportRequest request)
        {
            var sqlParameters = new[]
            {
                new SqlParameter("@date", request.fromDate),
        };
            return await GetDataTable("Download_KPITarget_Data", sqlParameters);
        }


    }
}
