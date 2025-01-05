using System;
using System.Collections.Generic;

namespace SIMAPI.Data.Entities
{
    public partial class Order
    {
        public int OrderId { get; set; }
        public int? UserId { get; set; }
        public int? ShopId { get; set; }        
        public decimal? NetAmount { get; set; }
        public decimal? VatAmount { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? DeliveryCharges { get; set; }
        public decimal? ItemTotal { get; set; }
        public decimal? TotalWithVATAmount { get; set; }
        public decimal? TotalWithOutVATAmount { get; set; }  
        public decimal? VatPercentage { get; set; }
        public decimal? DiscountPercentage { get; set; }
        
        public int? OrderStatusTypeId { get; set; }
        public int? OrderPaymentTypeId { get; set; }
        public int? OrderDeliveryTypeId { get; set; }
        public string? TrackingNumber { get; set; }
        public string? ShippingAddress { get; set; }
        public string? CouponCode { get; set; }
        public int? IsVatApplicable { get; set; }
        public int? IsRead { get; set; }
        public int? IsActive { get; set; }
        public string? ShippedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }

        public virtual IList<OrderDetail> OrderDetailsMaps { get; set; }

        public virtual IList<OrderHistory> OrderHistoryMaps { get; set; }
    }
}