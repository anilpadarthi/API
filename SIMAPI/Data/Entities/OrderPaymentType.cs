using System;
using System.Collections.Generic;

namespace SIMAPI.Data.Entities
{
    public partial class OrderPaymentType
    {
        public int OrderPaymentTypeId { get; set; }

        public string Name { get; set; } = null!;

        public bool? IsActive { get; set; }

    }
}