using System;

namespace SIMAPI.Data.Entities
{
    public class VwOrderPaymentHistory
    {
        public int? OrderId { get; set; }
        public int? OrderPaymentId { get; set; }
        public decimal? Amount { get; set; }
        public string? Status { get; set; }
        public bool? IsApproved { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
