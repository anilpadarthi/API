using SIMAPI.Data.Entities;

namespace SIMAPI.Data.Models
{
    public class ProductDetails
    {
        public Product product { get; set; }
        public IEnumerable<ProductPrice> productPrices { get; set; }
    }
}
