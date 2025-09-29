using System;
using System.Collections.Generic;

namespace SIMAPI.Data.Models
{

    public partial class CategoryDetails
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public string? Image { get; set; }

        public short? Status { get; set; }


    }
}
