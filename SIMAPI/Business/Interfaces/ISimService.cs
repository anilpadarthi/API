using SIMAPI.Data.Dto;
using SIMAPI.Data.Models;

namespace SIMAPI.Business.Interfaces
{
    public interface ISimService
    {

        Task<CommonResponse> ScanSimsAsync(GetSimInfoRequest request);
        Task<CommonResponse> AllocateSimsAsync(GetSimInfoRequest request);
        Task<CommonResponse> DeAllocateSimsAsync(GetSimInfoRequest request);
        Task<CommonResponse> GetSimHistoryDetailsAsync(GetSimInfoRequest request);
    }
}
