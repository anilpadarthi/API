using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models;

namespace SIMAPI.Repository.Interfaces
{
    public interface IBulkUploadRepository : IRepository
    {
        Task<BulkUploadFile> GetAreaByIdAsync(int id);
        Task<List<dynamic>> DownloadTargetDataAsync(GetReportRequest request);
    }
}
