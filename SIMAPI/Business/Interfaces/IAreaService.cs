using SIMAPI.Data.Dto;
using SIMAPI.Data.Models;

namespace SIMAPI.Business.Interfaces
{
    public interface IAreaService
    {
        Task<CommonResponse> GetByIdAsync(int id);
        Task<CommonResponse> GetByNameAsync(string name);
        Task<CommonResponse> GetAllAsync();       
        Task<CommonResponse> GetByPagingAsync(GetPagedSearch request);
        Task<CommonResponse> CreateAsync(AreaDto request);
        Task<CommonResponse> UpdateAsync(AreaDto request);
        Task<CommonResponse> DeleteAsync(int id);
        Task<CommonResponse> AllocateAreasToUserAsync(AllocateAreaDto request);
        Task<CommonResponse> DeAllocateAreasToUserAsync(int[] areaIds, int userId);

        Task<CommonResponse> GetAllAreasToAllocateAsync();

    }
}
