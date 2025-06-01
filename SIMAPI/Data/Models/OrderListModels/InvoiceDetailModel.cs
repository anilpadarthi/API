
namespace SIMAPI.Data.Models.OrderListModels
{
    public class InvoiceDetailModel
    {
        public int? OrderId { get; set; }
        public int? ShopId { get; set; }
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public string? AreaName { get; set; }
        public string? ShopName { get; set; }
        public string? ShopEmail { get; set; }
        public string? ShippedBy { get; set; }
        public string? ShippingAddress { get; set; }
        public string? PhoneNumber { get; set; }
        public decimal? ItemTotal { get; set; }
        public decimal?  NetAmount { get; set; }
        public decimal? VatAmount { get; set; }
        public decimal? TotalWithVATAmount { get; set; }
        public decimal? TotalWithOutVATAmount { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? DeliveryCharges { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? VatPercentage { get; set; }
        public string? TrackingNumber { get; set; }
        public int? OrderDeliveryTypeId { get; set; }
        public int? OrderStatusTypeId { get; set; }
        public int? OrderPaymentTypeId { get; set; }
        public string? OrderPaymentType { get; set; }
        public DateTime CreatedDate { get; set; }
        public short? IsVAT { get; set; }
        public IEnumerable<OrderItemModel>? Items { get; set; }
    }
}
