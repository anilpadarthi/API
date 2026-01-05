
using SIMAPI.Data.Entities;

namespace SIMAPI.Data.Models.OrderListModels
{
    public class InvoiceDetailModel
    {
        public int? OrderId { get; set; }
        public int? ShopId { get; set; }
        public int? OldShopId { get; set; }
        public int? UserId { get; set; }
        public string? UserName { get; set; }
        public string? AreaName { get; set; }
        public string? ShopName { get; set; }
        public string? ContactName { get; set; }
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


    public partial class ProductInfo
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductCode { get; set; }
        public int? CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public int? MixMatchGroupId { get; set; }
        public string? Description { get; set; }
        public string? Specification { get; set; }
        public string? ProductImage { get; set; }
        public bool? IsNewArrival { get; set; }
        public bool? IsBundle { get; set; }
        public bool? IsVatEnabled { get; set; }
        public bool? IsOutOfStock { get; set; }
        public short Status { get; set; }
        public decimal? BuyingPrice { get; set; }
        public decimal? SellingPrice { get; set; }
        public int? DisplayOrder { get; set; }
        public List<ProductPrice>? ProductPrices { get; set; }
        public List<ProductBundleDto>? BundleItems { get; set; }
    }
}
