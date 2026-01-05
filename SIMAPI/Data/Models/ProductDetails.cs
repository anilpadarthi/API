using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;

namespace SIMAPI.Data.Models
{
    public class ProductDetails
    {
        public Product product { get; set; }
        public IEnumerable<ProductPrice> productPrices { get; set; }
        public IEnumerable<ProductBundle>? BundleItems { get; set; }
        //public ProductCommission productCommission { get; set; }
    }
}
