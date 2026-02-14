using System;
using System.Collections.Generic;

namespace SIMAPI.Data.Entities
{

    public partial class OrderPayment
    {
        public int OrderPaymentId { get; set; }
        public int OrderId { get; set; }
        public int? ShopId { get; set; }
        public int? UserId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string? PaymentMode { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? ChequeNumber { get; set; }
        public string? ReferenceImage { get; set; }
        public string? CollectedStatus { get; set; }
        public string? Comments { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public short? Status { get; set; }
    }
}
