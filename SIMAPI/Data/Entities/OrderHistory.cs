using System;
using System.Collections.Generic;

namespace SIMAPI.Data.Entities
{
    public partial class OrderHistory
    {
        public int OrderHistoryId { get; set; }
        public int OrderId { get; set; }
        public int? OrderStatusTypeId { get; set; }
        public int? OrderPaymentTypeId { get; set; }
        public int? OrderDeliveryTypeId { get; set; }
        public string TrackingNumber { get; set; }
        public bool? IsActive { get; set; }
        public string? Comments { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
    }
}