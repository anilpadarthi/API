using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models;
using SIMAPI.Data.Models.Export;

namespace SIMAPI.Repository.Interfaces
{
    public interface ICategoryRepository : IRepository
    {
        Task<Category> GetCategoryByIdAsync(int categoryId);
        Task<CategoryDetails> GetCategoryDetailsByIdAsync(int categoryId);
        Task<Category> GetCategoryByNameAsync(string name);
        Task<IEnumerable<Category>> GetAllCategorysAsync();
        Task<IEnumerable<ExportCategory>> ExportAllCategoriesAsync();
        Task<IEnumerable<Category>> GetCategorysByPagingAsync(GetPagedSearch request);
        Task<int> GetTotalCategorysCountAsync(GetPagedSearch request);
    }
}
