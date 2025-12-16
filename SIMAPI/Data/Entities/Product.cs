using System;
using System.Collections.Generic;

namespace SIMAPI.Data.Entities
{
    public partial class Product
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? ProductCode { get; set; }
        public int? CategoryId { get; set; }
        public int? SubCategoryId { get; set; }
        public int? MixMatchGroupId { get; set; }
        public string? Description { get; set; }
        public string? Specification { get; set; }
        public string? ProductImage { get; set; }
        public bool? IsNewArrival { get; set; }
        public bool? IsBundle { get; set; }
        public bool? IsVatEnabled { get; set; }
        public bool? IsOutOfStock { get; set; }
        public short Status { get; set; }
        public decimal? BuyingPrice { get; set; }
        public decimal? SellingPrice { get; set; }
        public decimal? CommissionToAgent { get; set; }
        public decimal? CommissionToManager { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? CreatedBy { get; set; }
        public int? ModifiedBy { get; set; }
        public int? DisplayOrder { get; set; }
        public Category? Category { get; set; }
        public SubCategory? SubCategory { get; set; }
        public virtual ICollection<ProductPrice> ProductPrices { get; set; }
    }
}