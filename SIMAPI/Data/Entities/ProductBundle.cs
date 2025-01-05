using System;
using System.Collections.Generic;

namespace SIMAPI.Data.Entities
{
    public partial class ProductBundle
    {
        public int ProductBundleId { get; set; }
        public int ParentId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public bool IsActive { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? CreatedBy { get; set; }
        public int ModifiedBy { get; set; }
    }
}