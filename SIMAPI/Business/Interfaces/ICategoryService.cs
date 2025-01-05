using SIMAPI.Data.Dto;
using SIMAPI.Data.Models;

namespace SIMAPI.Business.Interfaces
{
    public interface ICategoryService
    {
        Task<CommonResponse> GetByIdAsync(int id);
        Task<CommonResponse> GetByNameAsync(string name);
        Task<CommonResponse> GetAllAsync();
        Task<CommonResponse> GetByPagingAsync(GetPagedSearch request);
        Task<CommonResponse> CreateAsync(CategoryDto request);
        Task<CommonResponse> UpdateAsync(CategoryDto request);
        Task<CommonResponse> DeleteAsync(int id);

    }
}
