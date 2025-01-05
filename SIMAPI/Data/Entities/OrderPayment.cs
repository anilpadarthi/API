using System;
using System.Collections.Generic;

namespace SIMAPI.Data.Entities
{

    public partial class OrderPayment
    {
        public int OrderPaymentId { get; set; }

        public int OrderId { get; set; }

        public decimal Amount { get; set; }

        public DateTime PaymentDate { get; set; }

        public bool IsActive { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? CreatedBy { get; set; }

        public int? ModifiedBy { get; set; }
    }
}
