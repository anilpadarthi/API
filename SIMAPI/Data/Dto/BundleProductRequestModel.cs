namespace SIMAPI.Data.Dto
{
    public partial class BundleProductRequestModel
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductCode { get; set; }
        public int? CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public string? Description { get; set; }
        public string? Specification { get; set; }
        public IFormFile ImageFile { get; set; }
        public string? Status { get; set; }
        public string? SalePrice { get; set; }
        public List<BundleProductModel>? BundleProducts { get; set; }

    }
}