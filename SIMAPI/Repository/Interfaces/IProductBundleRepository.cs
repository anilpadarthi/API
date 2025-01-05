using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;

namespace SIMAPI.Repository.Interfaces
{
    public interface IProductBundleRepository : IRepository
    {
        Task CreateAsync(ProductBundle request);
        Task UpdateAsync(ProductBundle request);
        Task DeleteAsync(int categoryId);
        Task<ProductBundle> GetByIdAsync(int categoryId);
        Task<IEnumerable<ProductBundle>> GetAllAsync();
        Task<IEnumerable<ProductBundle>> GetByPagingAsync(GetPagedSearch request);
    }
}
