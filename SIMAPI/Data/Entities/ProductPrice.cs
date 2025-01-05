using System;

namespace SIMAPI.Data.Entities
{
    public partial class ProductPrice
    {
        public int ProductPriceId { get; set; }

        public decimal SalePrice { get; set; }

        public int ProductId { get; set; }

        public int FromQty { get; set; }

        public int? ToQty { get; set; }

        public short? Status { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? CreatedBy { get; set; }

        public int? ModifiedBy { get; set; }

    }
}