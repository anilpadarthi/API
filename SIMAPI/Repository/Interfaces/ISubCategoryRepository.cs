using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models.Export;

namespace SIMAPI.Repository.Interfaces
{
    public interface ISubCategoryRepository : IRepository
    {
        Task<SubCategory> GetSubCategoryByIdAsync(int id);
        Task<SubCategory> GetSubCategoryByNameAsync(string name);
        Task<IEnumerable<SubCategory>> GetAllSubCategorysAsync();
        Task<IEnumerable<ExportSubCategory>> ExportAllSubCategoriesAsync();
        Task<IEnumerable<SubCategory>> GetSubCategorysByPagingAsync(GetPagedSearch request);
        Task<int> GetTotalSubCategorysCountAsync(GetPagedSearch request);
    }
}
