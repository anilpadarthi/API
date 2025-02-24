using System;

namespace SIMAPI.Data.Entities
{
    public class VwOrderPaymentHistory
    {
        public int? OrderId { get; set; }
        public int? OrderPaymentId { get; set; }
        public decimal? Amount { get; set; }
        public short? Status { get; set; }
        public string? CollectedStatus  { get; set; }
        public string? ReferenceNumber  { get; set; }
        public string? ReferenceImage  { get; set; }
        public string? Comments  { get; set; }
        public string? CollectedBy  { get; set; }
        public DateTime? PaymentDate { get; set; }
    }
}
