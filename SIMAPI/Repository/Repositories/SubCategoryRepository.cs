using Microsoft.EntityFrameworkCore;
using SIMAPI.Data;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Repository.Interfaces;
using SIMAPI.Business.Enums;

namespace SIMAPI.Repository.Repositories
{
    public class SubCategoryRepository : Repository, ISubCategoryRepository
    {
        public SubCategoryRepository(SIMDBContext context) : base(context)
        {
        }

        public async Task<IEnumerable<SubCategory>> GetAllSubCategorysAsync()
        {
            return await _context.Set<SubCategory>()
                .Where(w => w.Status != (short)EnumStatus.Deleted)
                .ToListAsync();
        }

        public async Task<SubCategory> GetSubCategoryByIdAsync(int id)
        {
            return await _context.Set<SubCategory>()
                .Where(w => w.SubCategoryId == id)
                .FirstOrDefaultAsync();
        }

        public async Task<SubCategory> GetSubCategoryByNameAsync(string name)
        {
            return await _context.Set<SubCategory>()
                .Where(w => w.SubCategoryName.ToUpper() == name.ToUpper())
                .Where(w => w.Status != (short)EnumStatus.Deleted)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<SubCategory>> GetSubCategorysByPagingAsync(GetPagedSearch request)
        {
            var query = _context.Set<SubCategory>().AsQueryable();
            query = query.Where(w => w.Status != (short)EnumStatus.Deleted);

            if (!string.IsNullOrEmpty(request.searchText))
            {
                query = query.Where(w => w.SubCategoryName.Contains(request.searchText));
            }


            var result = await query
                .OrderBy(o => o.SubCategoryName)
                .Skip((request.pageNo - 1) * request.pageSize)
                .Take(request.pageSize)
                .ToListAsync();

            return result;
        }

        public async Task<int> GetTotalSubCategorysCountAsync(GetPagedSearch request)
        {
            var query = _context.Set<SubCategory>().AsQueryable();
            query = query.Where(w => w.Status != (short)EnumStatus.Deleted);

            if (!string.IsNullOrEmpty(request.searchText))
            {
                query = query.Where(w => w.SubCategoryName.Contains(request.searchText));
            }
            return await query.CountAsync();
        }

        
    }
}
