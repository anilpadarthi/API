namespace SIMAPI.Data.Dto
{
    public class OrderProductModel
    {
        public int ProductId { get; set; }
        public int Qty { get; set; }
        public decimal? SalePrice { get; set; }
        public int? ProductColourId { get; set; }
        public int? ProductSizeId { get; set; }
    }
}
