
namespace SIMAPI.Data.Models.Export
{
    public class ProductExportDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public decimal? BuyingPrice { get; set; }
        public decimal? SellingPrice { get; set; }
        public int? DisplayOrder { get; set; }
        public int? Status { get; set; }
        public decimal? CommissionToAgent { get; set; }
        public decimal? CommissionToManager { get; set; }
    }
}
