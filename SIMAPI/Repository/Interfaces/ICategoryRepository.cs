using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;

namespace SIMAPI.Repository.Interfaces
{
    public interface ICategoryRepository : IRepository
    {
        Task<Category> GetCategoryByIdAsync(int id);
        Task<Category> GetCategoryByNameAsync(string name);
        Task<IEnumerable<Category>> GetAllCategorysAsync();
        Task<IEnumerable<Category>> GetCategorysByPagingAsync(GetPagedSearch request);
        Task<int> GetTotalCategorysCountAsync(GetPagedSearch request);
    }
}
