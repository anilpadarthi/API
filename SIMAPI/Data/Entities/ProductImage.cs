using System;
using System.Collections.Generic;

namespace SIMAPI.Data.Entities
{
    public partial class ProductImage
    {
        public int ProductImageId { get; set; }

        public string Image { get; set; }

        public int ProductId { get; set; }

        public short Status { get; set; }

        public DateTime? CreatedDate { get; set; }

    }
}