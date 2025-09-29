using SIMAPI.Data.Entities;

namespace SIMAPI.Data.Models.OrderListModels
{
    public class OrderItemModel
    {
        public int? OrderId { get; set; }
        public int? OrderDetailId { get; set; }
        public int? ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? CategoryName { get; set; }
        public string? SubCategoryName { get; set; }
        public string? ProductCode { get; set; }
        public int? Qty { get; set; }
        public decimal? SalePrice { get; set; }
        public int? ProductSizeId { get; set; }
        public int? ProductColourId { get; set; }
        public string? ProductImage { get; set; }
        public int? IsBundle { get; set; }
        public int? MixMatchGroupId { get; set; }
        public List<ProductPrice> ProductPrices { get; set; }
    }
}
