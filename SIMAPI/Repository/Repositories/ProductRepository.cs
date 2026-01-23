using DocumentFormat.OpenXml.Office2010.Excel;
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

        public async Task UpdateStatusAsync(int id, bool status)
        {
            var dbRecord = await GetByIdAsync(id);
            dbRecord.Status = status ? (short)EnumStatus.Active : (short)EnumStatus.InActive;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDisplayOrderAsync(int id, int displayOrder)
        {
            var dbRecord = await GetByIdAsync(id);
            dbRecord.DisplayOrder = displayOrder;
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

        public async Task<IEnumerable<ProductBundle>> GetProductBundleByIdAsync(int bundleProductId)
        {
            return await _context.Set<ProductBundle>()
                .Where(w => w.BundleProductId == bundleProductId && w.IsActive == true)
                .ToListAsync();
        }

        public async Task<IEnumerable<ProductBundleDto>> GetBundleItemsAsync(int bundleProductId)
        {
            var result = await (from b in _context.Set<ProductBundle>()
                                join p in _context.Set<Product>() on b.ProductId equals p.ProductId into pj
                                from p in pj.DefaultIfEmpty()
                                where b.BundleProductId == bundleProductId && b.IsActive == true
                                orderby b.DisplayOrder
                                select new ProductBundleDto
                                {
                                    ProductId = b.ProductId,
                                    ProductName = p != null ? p.ProductName : string.Empty,
                                    Quantity = b.Quantity,
                                    Price = b.Price?? 0
                                }).ToListAsync();

            return result;
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
            productDetails.BundleItems = await GetProductBundleByIdAsync(productId);
            //productDetails.productCommission = await GetProductCommissionByIdAsync(productId);

            return productDetails;
        }

        public async Task<IEnumerable<ProductPrice>> GetProductPricesAsync(int productId)
        {
            return await _context.Set<ProductPrice>()
                .Where(w => w.ProductId == productId && w.Status != (int)EnumStatus.Deleted)
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
            query = query.Where(w => w.Status != (int)EnumStatus.Deleted
            && w.Status == (request.isActive.Value ? (int)EnumStatus.Active : (int)EnumStatus.InActive));

            if (!string.IsNullOrEmpty(request.searchText))
            {
                if (int.TryParse(request.searchText, out int productId))
                {
                    // If numeric → search by ID
                    query = query.Where(w => w.ProductId == productId);
                }
                else
                {
                    query = query
                        .Where(w => w.ProductName.Contains(request.searchText)
                               || w.ProductCode.Contains(request.searchText));
                }
            }
            if (request.categoryId.HasValue)
            {
                query = query.Where(w => w.CategoryId == request.categoryId);

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
            query = query.Where(w => w.Status != (int)EnumStatus.Deleted
            && w.Status == (request.isActive.Value ? (int)EnumStatus.Active : (int)EnumStatus.InActive));

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

        public async Task<ProductCommission?> GetProductCommissionByIdAsync(int productId)
        {
            return await _context.Set<ProductCommission>()
                .Where(w => w.ProductId == productId && w.IsActive == 1)
                .FirstOrDefaultAsync();
        }

    }
}
