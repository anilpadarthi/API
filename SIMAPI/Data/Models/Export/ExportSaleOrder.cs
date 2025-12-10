
namespace SIMAPI.Data.Models.Export
{
    public class ExportSaleOrder
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public string? UserName { get; set; }
        public int AreaId { get; set; }
        public string? AreaName { get; set; }
        public int OldShopId { get; set; }
        public string? ShopName { get; set; }
        public decimal? ItemTotal { get; set; }
        public decimal? ExpectedAmount { get; set; }
        public short? IsVAT { get; set; }
        public decimal? DeliveryCharges { get; set; }
        public string? OrderStatus { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Courier { get; set; }
        public string? TrackingNumber { get; set; }
        public string? ShippedBy { get; set; }
        
        public decimal? CollectedAmount { get; set; }
        public string? CollectedStatus { get; set; }
        public decimal? WalletAmount { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
