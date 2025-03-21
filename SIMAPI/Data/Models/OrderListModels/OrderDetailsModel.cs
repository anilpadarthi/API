
namespace SIMAPI.Data.Models.OrderListModels
{
    public class OrderDetailsModel
    {
        public int? OrderId { get; set; }
        public int? ShopId { get; set; }
        public int? UserId { get; set; }
        public decimal?  NetAmount { get; set; }
        public decimal? VatAmount { get; set; }
        public decimal? TotalWithVATAmount { get; set; }
        public decimal? TotalWithOutVATAmount { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? DeliveryCharges { get; set; }
        public decimal? DiscountPercentage { get; set; }
        public decimal? VatPercentage { get; set; }
        public string? ShippingAddress { get; set; }
        public string? TrackingNumber { get; set; }
        public int? OrderDeliveryTypeId { get; set; }
        public int? OrderStatusTypeId { get; set; }
        public int? OrderPaymentTypeId { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<OrderItemModel>? Items { get; set; }
    }
}
