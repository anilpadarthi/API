using SIMAPI.Data.Dto;
using SIMAPI.Data.Models;

namespace SIMAPI.Business.Interfaces
{
    public interface IDownloadService
    {
        Task<Stream?> DownloadInstantActivationListAsync(GetReportRequest request);
    }

}
