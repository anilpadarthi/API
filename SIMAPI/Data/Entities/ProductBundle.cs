using System;
using System.Collections.Generic;

namespace SIMAPI.Data.Entities
{
    public partial class ProductBundle
    {
        public int? ProductBundleId { get; set; }
        public int BundleProductId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int DisplayOrder { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }
    }

    public class ProductBundleDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}