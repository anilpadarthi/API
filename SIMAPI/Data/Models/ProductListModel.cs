namespace SIMAPI.Data.Models
{
    public class ProductListModel
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductCode { get; set; }
        public int? CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public string? Status { get; set; }
        public string? CategoryName { get; set; }
        public string? SubCategoryName { get; set; }
        public decimal? SalePrice { get; set; }
        public string? Image { get; set; }
        public int? DisplayOrder { get; set; }
        public int? TotalCount { get; set; }
    }
}
