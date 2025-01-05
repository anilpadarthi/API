using Microsoft.EntityFrameworkCore;
using SIMAPI.Data;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Repository.Interfaces;

namespace SIMAPI.Repository.Repositories
{
    public class ProductBundleRepository : Repository, IProductBundleRepository
    {
        public ProductBundleRepository(SIMDBContext context) : base(context)
        {
        }

        public async Task CreateAsync(ProductBundle request)
        {
            _context.Add(request);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(ProductBundle request)
        {
            var category = await _context.Set<ProductBundle>()
                .Where(w => w.ProductBundleId == request.ProductBundleId)
                .FirstOrDefaultAsync();
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int categoryId)
        {
            var category = await GetByIdAsync(categoryId);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductBundle>> GetAllAsync()
        {
            return await _context.Set<ProductBundle>()
                .ToListAsync();
        }

        public async Task<ProductBundle> GetByIdAsync(int categoryId)
        {
            return await _context.Set<ProductBundle>()
                .Where(w => w.ProductBundleId == categoryId)
                .FirstOrDefaultAsync();
        }



        public async Task<IEnumerable<ProductBundle>> GetByPagingAsync(GetPagedSearch request)
        {
            return await _context.Set<ProductBundle>()
                .Where(w => w.IsActive == true)
                .ToListAsync();
        }


    }
}
