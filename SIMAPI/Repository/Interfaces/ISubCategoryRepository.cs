using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;

namespace SIMAPI.Repository.Interfaces
{
    public interface ISubCategoryRepository : IRepository
    {
        Task<SubCategory> GetSubCategoryByIdAsync(int id);
        Task<SubCategory> GetSubCategoryByNameAsync(string name);
        Task<IEnumerable<SubCategory>> GetAllSubCategorysAsync();
        Task<IEnumerable<SubCategory>> GetSubCategorysByPagingAsync(GetPagedSearch request);
        Task<int> GetTotalSubCategorysCountAsync(GetPagedSearch request);
    }
}
