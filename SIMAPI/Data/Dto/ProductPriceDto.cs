using System;
using System.Collections.Generic;

namespace SIMAPI.Data.Dto
{
    public partial class ProductPriceDto
    {
        public int? ProductPriceId { get; set; }
        public int? ProductId { get; set; }
        public decimal? SalePrice { get; set; }
        public int? FromQty { get; set; }
        public int? ToQty { get; set; }
        public bool? IsActive { get; set; }

    }
}
