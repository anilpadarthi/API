using SIMAPI.Data.Dto;
using SIMAPI.Data.Models;

namespace SIMAPI.Business.Interfaces
{
    public interface IProductService
    {
        //Product
        Task<CommonResponse> CreateAsync(ProductDto request);
        Task<CommonResponse> AddProductImageAsync(ProductImageModel request);
        Task<CommonResponse> CreateBundleProductAsync(BundleProductRequestModel request);
        Task<CommonResponse> UpdateAsync(ProductDto request);
        Task<CommonResponse> UpdateStatusAsync(int id, string status);
        Task<CommonResponse> GetByIdAsync(int id);
        Task<CommonResponse> GetAllAsync();
        Task<CommonResponse> GetByPagingAsync(GetPagedSearch request);
        Task<CommonResponse> GetAllProductsAsync(ProductSearchModel request);
        Task<CommonResponse> DeleteProductAsync(int id);

        ////Product Price
        //Task<CommonResponse> CreateProductPriceAsync(ProductPriceMap request);
        //Task<CommonResponse> UpdateProductPriceAsync(ProductPriceMap request);
        //Task<CommonResponse> DeleteProductPriceAsync(int productPriceId);
        //Task<CommonResponse> GetAllProductPricesAsync(int productId);

        ////Product Bundle
        //Task<CommonResponse> CreateProductBundleAsync(ProductBundle request);
        //Task<CommonResponse> UpdateProductBundleAsync(ProductBundle request);
        //Task<CommonResponse> DeleteProductBundleAsync(int productBundleId);
        //Task<CommonResponse> GetAllProductBundleAsync(int productId);

        ////Product Size
        //Task<CommonResponse> CreateProductSizeAsync(ProductSizeMap request);
        //Task<CommonResponse> UpdateProductSizeAsync(ProductSizeMap request);
        //Task<CommonResponse> DeleteProductSizeAsync(int productSizeId);
        //Task<CommonResponse> GetAllProductSizesAsync();

        ////Product Colour
        //Task<CommonResponse> CreateProductColourAsync(ProductColourMap request);
        //Task<CommonResponse> UpdateProductColourAsync(ProductColourMap request);
        //Task<CommonResponse> DeleteProductColourAsync(int productColourId);
        //Task<CommonResponse> GetAllProductColoursAsync();
    }
}
