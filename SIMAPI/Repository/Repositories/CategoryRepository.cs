using Microsoft.EntityFrameworkCore;
using SIMAPI.Data;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Repository.Interfaces;
using SIMAPI.Business.Enums;
using SIMAPI.Data.Models;

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

        public async Task<CategoryDetails> GetCategoryDetailsByIdAsync(int categoryId)
        {
            var category = await _context.Set<Category>()
                .Where(w => w.CategoryId == categoryId)
                .FirstOrDefaultAsync();
            CategoryDetails categoryDetails = new CategoryDetails();
            if (category != null)
            {
                categoryDetails.CategoryId = category.CategoryId;
                categoryDetails.CategoryName = category.CategoryName;
                categoryDetails.Status = category.Status;
                categoryDetails.Image = category.Image;
                var categoryCommissionDetails = await GetCategoryCommissionByIdAsync(category.CategoryId);
                categoryDetails.CommissionPercent = categoryCommissionDetails.CommissionPercent;
            }
            return categoryDetails;
        }

        public async Task<Category> GetCategoryByIdAsync(int categoryId)
        {
            return await _context.Set<Category>()
                .Where(w => w.CategoryId == categoryId)
                .FirstOrDefaultAsync();

        }

        public async Task<CategoryCommission> GetCategoryCommissionByIdAsync(int categoryId)
        {
            return await _context.Set<CategoryCommission>()
                .Where(w => w.CategoryId == categoryId && w.IsActive == 1)
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
