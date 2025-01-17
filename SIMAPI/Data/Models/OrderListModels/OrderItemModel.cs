namespace SIMAPI.Data.Models.OrderListModels
{
    public class OrderItemModel
    {
        public int? OrderId { get; set; }
        public int? OrderDetailId { get; set; }
        public int? ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductCode { get; set; }
        public int? Qty { get; set; }
        public decimal? SalePrice { get; set; }
        public int? ProductSizeId { get; set; }
        public int? ProductColourId { get; set; }
    }
}
