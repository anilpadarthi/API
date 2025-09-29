using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models;

namespace SIMAPI.Repository.Interfaces
{
    public interface ICategoryRepository : IRepository
    {
        Task<Category> GetCategoryByIdAsync(int categoryId);
        Task<CategoryDetails> GetCategoryDetailsByIdAsync(int categoryId);
        Task<Category> GetCategoryByNameAsync(string name);
        Task<IEnumerable<Category>> GetAllCategorysAsync();
        Task<IEnumerable<Category>> GetCategorysByPagingAsync(GetPagedSearch request);
        Task<int> GetTotalCategorysCountAsync(GetPagedSearch request);
    }
}
