using System;

namespace SIMAPI.Data.Entities
{
    public class VwOrderHistory
    {
        public int? OrderId { get; set; }
        public int? OrderHistoryId { get; set; }
        public string? Comments { get; set; }
        public string? OrderStatus { get; set; }
        public string? PaymentMethod { get; set; }
        public string? ShippingMode { get; set; }
        public string? TrackingNumber { get; set; }
        public string? CreatedByName { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
