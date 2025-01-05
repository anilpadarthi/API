using System;

namespace SIMAPI.Data.Entities
{
    public partial class ProductColour
    {
        public int ProductColourId { get; set; }

        public int ColourId { get; set; }

        public int ProductId { get; set; }

        public bool IsActive { get; set; }

        public DateTime? CreatedDate { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public int? CreatedBy { get; set; }

        public int? ModifiedBy { get; set; }

        public virtual Colour Colour { get; set; } = null!;

        public virtual Product Product { get; set; } = null!;
    }
}