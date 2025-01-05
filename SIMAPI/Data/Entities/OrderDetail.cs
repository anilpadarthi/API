using System;
using System.Collections.Generic;

namespace SIMAPI.Data.Entities
{
    public partial class OrderDetail
    {
        public int OrderDetailId { get; set; }

        public int OrderId { get; set; }

        public int? ProductId { get; set; }

        public int? Qty { get; set; }

        public decimal? SalePrice { get; set; }

        public int? ProductColourId { get; set; }

        public int? ProductSizeId { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? CreatedBy { get; set; }

        public int? ModifiedBy { get; set; }
    }
}