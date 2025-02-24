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
    public class ManagementRepository : Repository, IManagementRepository
    {
        public ManagementRepository(SIMDBContext context) : base(context)
        {
        }

        

        public async Task<BulkUploadFile> GetAreaByIdAsync(int id)
        {
            return await _context.Set<BulkUploadFile>()
                .Where(w => w.BulkUploadFileId == id)
                .FirstOrDefaultAsync();
        }

       
    }
}
