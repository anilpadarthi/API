using SIMAPI.Data.Dto;
using SIMAPI.Data.Models;

namespace SIMAPI.Business.Interfaces
{
    public interface ISubCategoryService
    {
        Task<CommonResponse> GetByIdAsync(int id);
        Task<CommonResponse> GetByNameAsync(string name);
        Task<CommonResponse> GetAllAsync();
        Task<CommonResponse> GetByPagingAsync(GetPagedSearch request);
        Task<CommonResponse> CreateAsync(SubCategoryDto request);
        Task<CommonResponse> UpdateAsync(SubCategoryDto request);
        Task<CommonResponse> DeleteAsync(int id);

    }
}
