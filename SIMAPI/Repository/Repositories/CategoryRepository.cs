using Microsoft.EntityFrameworkCore;
using SIMAPI.Data;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Repository.Interfaces;
using SIMAPI.Business.Enums;

namespace SIMAPI.Repository.Repositories
{
    public class CategoryRepository : Repository, ICategoryRepository
    {
        public CategoryRepository(SIMDBContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Category>> GetAllCategorysAsync()
        {
            return await _context.Set<Category>()
                .Where(w => w.Status != (short)EnumStatus.Deleted)
                .ToListAsync();
        }

        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await _context.Set<Category>()
                .Where(w => w.CategoryId == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Category> GetCategoryByNameAsync(string name)
        {
            return await _context.Set<Category>()
                .Where(w => w.CategoryName.ToUpper() == name.ToUpper())
                .Where(w => w.Status != (short)EnumStatus.Deleted)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Category>> GetCategorysByPagingAsync(GetPagedSearch request)
        {
            var query = _context.Set<Category>().AsQueryable();
            query = query.Where(w => w.Status != (short)EnumStatus.Deleted);

            if (!string.IsNullOrEmpty(request.searchText))
            {
                query = query.Where(w => w.CategoryName.Contains(request.searchText));
            }


            var result = await query
                .OrderBy(o => o.CategoryName)
                .Skip((request.pageNo - 1) * request.pageSize)
                .Take(request.pageSize)
                .ToListAsync();

            return result;
        }

        public async Task<int> GetTotalCategorysCountAsync(GetPagedSearch request)
        {
            var query = _context.Set<Category>().AsQueryable();
            query = query.Where(w => w.Status != (short)EnumStatus.Deleted);

            if (!string.IsNullOrEmpty(request.searchText))
            {
                query = query.Where(w => w.CategoryName.Contains(request.searchText));
            }
            return await query.CountAsync();
        }

       
    }
}
