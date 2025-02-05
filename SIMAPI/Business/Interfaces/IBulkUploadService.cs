using SIMAPI.Data.Dto;
using SIMAPI.Data.Models;

namespace SIMAPI.Business.Interfaces
{
    public interface IBulkUploadService
    {

        Task<CommonResponse> UploadFile(BulkUploadDto request);
    }
}
