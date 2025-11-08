using System.ComponentModel.DataAnnotations.Schema;

namespace SIMAPI.Data.Entities
{
    public partial class VwOrders
    {
        public int? OrderId { get; set; }
        public int? UserId { get; set; }
        public int? MonitorBy { get; set; }
        public int? AreaId { get; set; }
        public int? ShopId { get; set; }
        public string? UserName { get; set; }
        public string? AreaName { get; set; }
        public string? ShopName { get; set; }
        public decimal? ItemTotal { get; set; }
        public decimal? NetAmount { get; set; }
        public decimal? TotalWithVATAmount { get; set; }
        public decimal? TotalWithOutVATAmount { get; set; }
        public string? OrderStatus { get; set; }
        public int? OrderStatusId { get; set; }
        public string? PaymentMethod { get; set; }
        public int? PaymentMethodId { get; set; }
        public string? Courier { get; set; }
        public int? ShippingModeId { get; set; }
        public int? UnpaidCount { get; set; }
        public string? TrackingNumber { get; set; }
        public string? ShippedBy { get; set; }
        public string? RequestType { get; set; }        
        public string? ColourName { get; set; }        
        public short? IsVAT { get; set; }
        public bool? IsHide { get; set; }
        public decimal? ExpectedAmount { get; set; }
        public decimal? CollectedAmount { get; set; }
        public string? CollectedStatus { get; set; }
        public decimal? WalletAmount { get; set; }
        public decimal? DeliveryCharges { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
