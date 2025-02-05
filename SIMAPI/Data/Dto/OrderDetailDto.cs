
namespace SIMAPI.Data.Dto
{
    public partial class OrderDetailDto
    {
        public int? orderId { get; set; }
        public int? userId { get; set; }
        public int? shopId { get; set; }
        public decimal? itemTotal { get; set; }
        public decimal? vatAmount { get; set; }
        public decimal? deliveryCharges { get; set; }
        public decimal? discountAmount { get; set; }
        public decimal? totalWithVATAmount { get; set; }
        public decimal? totalWithOutVATAmount { get; set; }
        public string? couponCode { get; set; }
        public int? orderStatusId { get; set; }
        public int? paymentMethodId { get; set; }
        public int? shippingModeId { get; set; }
        public string? trackingNumber { get; set; }
        public string? shippingAddress { get; set; }
        public decimal? vatPercentage { get; set; }
        public decimal? discountPercentage { get; set; }
        public decimal? walletAmount { get; set; }
        public int? loggedInUserId { get; set; }
        public int? placedBy { get; set; }
        public string? requestType { get; set; }
        public List<OrderProductModel> items { get; set; }

    }
}
