using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models;

namespace SIMAPI.Business.Interfaces
{
    public interface IProductBundleService
    {
        Task<CommonResponse> CreateAsync(ProductBundle request);
        Task<CommonResponse> UpdateAsync(ProductBundle request);
        Task<CommonResponse> DeleteAsync(int categoryId);
        Task<CommonResponse> GetByIdAsync(int categoryId);
        Task<CommonResponse> GetAllAsync();
        Task<CommonResponse> GetByPagingAsync(GetPagedSearch request);
    }
}
