using SIMAPI.Data.Entities;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Models;

namespace SIMAPI.Business.Interfaces
{
    public interface IMixMatchGroupService
    {
        Task<CommonResponse> GetByIdAsync(int id);
        Task<CommonResponse> GetByNameAsync(string name);
        Task<CommonResponse> GetByPagingAsync(GetPagedSearch request);
        Task<CommonResponse> CreateAsync(MixMatchGroup request);
        Task<CommonResponse> UpdateAsync(MixMatchGroup request);
        Task<CommonResponse> DeleteAsync(int id);

    }
}
