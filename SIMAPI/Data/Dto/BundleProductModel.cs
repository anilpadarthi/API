namespace SIMAPI.Data.Dto
{
    public partial class BundleProductModel
    {
        public int? ProductBundleId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal? SalePrice { get; set; }

        public bool? IsActive { get; set; }

    }
}
