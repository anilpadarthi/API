using System;
using System.Collections.Generic;

namespace SIMAPI.Data.Entities
{
    public partial class ProductImage
    {
        public int ProductImageId { get; set; }

        public string Image { get; set; }
        public string Type { get; set; }

        public int ProductId { get; set; }

        public bool IsActive { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? CreatedBy { get; set; }

        public int? ModifiedBy { get; set; }

        public virtual Product Product { get; set; } = null!;
    }
}