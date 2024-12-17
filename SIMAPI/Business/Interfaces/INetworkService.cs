using SIMAPI.Data.Dto;
using SIMAPI.Data.Models;

namespace SIMAPI.Business.Interfaces
{
    public interface INetworkService
    {
        Task<CommonResponse> GetByIdAsync(int id);
        Task<CommonResponse> GetByNameAsync(string name);
        Task<CommonResponse> GetAllAsync();
        Task<CommonResponse> GetByPagingAsync(GetPagedSearch request);
        Task<CommonResponse> CreateAsync(NetworkDto request);
        Task<CommonResponse> UpdateAsync(NetworkDto request);
        Task<CommonResponse> DeleteAsync(int id);

    }
}
