using System;

namespace SIMAPI.Data.Entities
{
    public partial class SubCategory
    {
        public int SubCategoryId { get; set; }

        public string SubCategoryName { get; set; }

        public int CategoryId { get; set; }

        public string? Image { get; set; }

        public short? Status { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }
        public int? DisplayOrder { get; set; }

        public int? CreatedBy { get; set; }

        public int? ModifiedBy { get; set; }
    }
}