using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SIMAPI.Business.Enums;
using SIMAPI.Data;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models;
using SIMAPI.Repository.Interfaces;

namespace SIMAPI.Repository.Repositories
{
    public class ProductRepository : Repository, IProductRepository
    {
        public ProductRepository(SIMDBContext context) : base(context)
        {
        }

        public async Task CreateAsync(Product request)
        {
            _context.Add(request);
            await _context.SaveChangesAsync();

        }
        public async Task UpdateAsync(Product request)
        {
            var dbRecord = await _context.Set<Product>().Where(w => w.CategoryId == request.CategoryId).FirstOrDefaultAsync();
            dbRecord.ProductName = request.ProductName;
            dbRecord.ProductCode = request.ProductCode;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateStatusAsync(int id, string status)
        {
            var dbRecord = await GetByIdAsync(id);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Set<Product>()
                .Where(cat => cat.Status == (int)EnumStatus.Active)
                .ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int productId)
        {
            return await _context.Set<Product>()
                .Where(w => w.ProductId == productId)
                .FirstOrDefaultAsync();
        }

        public async Task<ProductPrice> GetProductPriceByIdAsync(int productPriceId)
        {
            return await _context.Set<ProductPrice>()
                .Where(w => w.ProductPriceId == productPriceId)
                .FirstOrDefaultAsync();
        }

        public async Task<ProductBundle> GetProductBundleByIdAsync(int productBundleId)
        {
            return await _context.Set<ProductBundle>()
                .Where(w => w.ProductBundleId == productBundleId)
                .FirstOrDefaultAsync();
        }

        public async Task<Product> GetByNameAsync(string productName)
        {
            return await _context.Set<Product>()
                .Where(w => w.ProductName.ToUpper() == productName.ToUpper())
                .FirstOrDefaultAsync();
        }

        public async Task<ProductDetails?> GetProductDetailsAsync(int productId)
        {
            ProductDetails productDetails = new ProductDetails();
            productDetails.product = await _context.Set<Product>()
                    .Where(w => w.ProductId == productId)
                    .FirstOrDefaultAsync();
            productDetails.productPrices = await GetProductPricesAsync(productId);

            return productDetails;
        }

        public async Task<IEnumerable<ProductPrice>> GetProductPricesAsync(int productId)
        {
            return await _context.Set<ProductPrice>()
                .Where(w => w.ProductId == productId)
                    .ToListAsync();
        }


        public async Task<Product> GetProductAsync(int productId)
        {
            var result = await _context.Set<Product>()
                .Where(w => w.ProductId == productId)
                .FirstOrDefaultAsync();
            return result;
        }

        public async Task<IEnumerable<Product>> GetByPagingAsync(GetPagedSearch request)
        {
            var query = _context.Set<Product>()
                        .Include(i => i.Category)
                        .Include(i => i.SubCategory)
                        .AsQueryable();
            query = query.Where(w => w.Status != (int)EnumStatus.Deleted);
            if (!string.IsNullOrEmpty(request.searchText))
            {
                query = query
                        .Where(w => w.ProductName.Contains(request.searchText)
                               || w.ProductCode.Contains(request.searchText));
            }
            if (request.categoryId.HasValue)
            {
                query = query.Where(w=>w.CategoryId == request.categoryId);

            }
            if (request.subCategoryId.HasValue)
            {
                query = query.Where(w => w.SubCategoryId == request.subCategoryId);
            }

            var result = await query
                .OrderBy(o => o.ProductName)
                .Skip((request.pageNo - 1) * request.pageSize)
                .Take(request.pageSize)
                .ToListAsync();

            return result;
        }

        public async Task<int> GetTotalProductsCountAsync(GetPagedSearch request)
        {
            var query = _context.Set<Product>()
                        .Include(i => i.Category)
                        .Include(i => i.SubCategory)
                        .AsQueryable();
            query = query.Where(w => w.Status != (int)EnumStatus.Deleted);
            if (!string.IsNullOrEmpty(request.searchText))
            {
                query = query
                        .Where(w => w.ProductName.Contains(request.searchText)
                               || w.ProductCode.Contains(request.searchText));
            }
            if (request.categoryId.HasValue)
            {
                query = query.Where(w => w.CategoryId == request.categoryId);

            }
            if (request.subCategoryId.HasValue)
            {
                query = query.Where(w => w.SubCategoryId == request.subCategoryId);
            }
            return await query.CountAsync();
        }

        public async Task<IEnumerable<ProductListModel>> GetAllProductsAsync(ProductSearchModel request)
        {
            var sqlParameters = new[]
            {
                request.categoryId.HasValue ? new SqlParameter("@categoryId", request.categoryId.Value) : new SqlParameter("@categoryId", DBNull.Value),
                request.subCategoryId.HasValue ? new SqlParameter("@subCategoryId", request.subCategoryId.Value) : new SqlParameter("@subCategoryId", DBNull.Value),
                request.searchText !=null ? new SqlParameter("@searchText", request.searchText) : new SqlParameter("@searchText", DBNull.Value),
            };
            return await ExecuteStoredProcedureAsync<ProductListModel>("exec usp_GetAllProductList @categoryId,@subCategoryId,@searchText", sqlParameters);
        }

        public async Task<int> GetTotalCountAsync(GetPagedSearch request)
        {
            var query = _context.Set<Product>()
               .Where(w => w.Status == (int)EnumStatus.Active);

            if (request.categoryId.HasValue && request.categoryId > 0)
            {
                query = query.Where(w => w.CategoryId == request.categoryId);
            }
            if (request.subCategoryId.HasValue && request.subCategoryId > 0)
            {
                query = query.Where(w => w.SubCategoryId == request.subCategoryId);
            }
            if (!string.IsNullOrEmpty(request.searchText))
            {
                query = query.Where(w => w.ProductName.Contains(request.searchText) || w.ProductCode.Contains(request.searchText));
            }


            return await query.CountAsync();
        }

    }
}
