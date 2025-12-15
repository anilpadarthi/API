using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models;

namespace SIMAPI.Repository.Interfaces
{
    public interface IProductRepository: IRepository
    {
        Task CreateAsync(Product request);
        Task UpdateAsync(Product request);
        Task UpdateStatusAsync(int id, bool status);
        Task<Product> GetByIdAsync(int productId);
        Task<ProductPrice> GetProductPriceByIdAsync(int productPriceId);
        Task<ProductBundle> GetProductBundleByIdAsync(int productBundleId);
        Task<Product> GetByNameAsync(string productName);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<IEnumerable<Product>> GetByPagingAsync(GetPagedSearch request);
        Task<IEnumerable<ProductListModel>> GetAllProductsAsync(ProductSearchModel request);
        Task<int> GetTotalCountAsync(GetPagedSearch request);
        Task<ProductDetails?> GetProductDetailsAsync(int productId);
        Task<IEnumerable<ProductPrice>> GetProductPricesAsync(int productId);
        Task<int> GetTotalProductsCountAsync(GetPagedSearch request);
        Task<ProductCommission?> GetProductCommissionByIdAsync(int productId);
    }
}
