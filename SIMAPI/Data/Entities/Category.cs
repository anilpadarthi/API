using System;
using System.Collections.Generic;

namespace SIMAPI.Data.Entities
{

    public partial class Category
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string? Image { get; set; }

        public short? Status { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? CreatedBy { get; set; }

        public int? ModifiedBy { get; set; }
        public int? DisplayOrder { get; set; }

    }
}
