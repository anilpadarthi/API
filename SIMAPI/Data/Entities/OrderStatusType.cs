using System;
using System.Collections.Generic;

namespace SIMAPI.Data.Entities
{
    public partial class OrderStatusType
    {
        public int OrderStatusTypeId { get; set; }

        public string Name { get; set; } = null!;

        public bool? IsActive { get; set; }
    }
}