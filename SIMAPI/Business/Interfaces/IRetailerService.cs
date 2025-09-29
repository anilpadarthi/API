using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models;

namespace SIMAPI.Business.Interfaces
{
    public interface IRetailerService
    {
        Task<CommonResponse> GetActvationsAsync(GetReportRequest request);
        Task<CommonResponse> GetCommissionsAsync(GetReportRequest request);
        Task<CommonResponse> GetStockVsConnectionsAsync(GetReportRequest request);
    }
}
