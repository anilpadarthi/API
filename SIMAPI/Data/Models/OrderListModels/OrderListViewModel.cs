using System;

namespace SIMAPI.Data.Models.OrderListModels
{
    public class OrderListViewModel
    {
        public int? OrderId { get; set; }
        public int? UserId { get; set; }
        public int? AreaId { get; set; }
        public int? ShopId { get; set; }
        public string? UserName { get; set; }
        public string? AreaName { get; set; }
        public string? ShopName { get; set; }
        public decimal? NetAmount { get; set; }
        public decimal? TotalWithVATAmount { get; set; }
        public decimal? TotalWithOutVATAmount { get; set; }
        public string? OrderStatus { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Courier { get; set; }
        public string? TrackingNumber { get; set; }
        public string? ShippedBy { get; set; }
        public int? IsVATApplicable { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? TotalOrdersCount { get; set; }

    }
}
