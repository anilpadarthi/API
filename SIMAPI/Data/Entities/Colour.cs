using System;
using System.Collections.Generic;

namespace SIMAPI.Data.Entities
{
    public partial class Colour
    {
        public int ColourId { get; set; }

        public string ColourName { get; set; } = null!;

        public string ColourCode { get; set; } = null!;

        public bool? IsActive { get; set; }

        public DateTime? CreatedDate { get; set; }

        public int? CreatedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public virtual ICollection<ProductColour> ProductColourMaps { get; } = new List<ProductColour>();
    }
}
