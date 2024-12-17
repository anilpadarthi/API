namespace SIMAPI.Data.Entities
{
    public partial class SupplierProduct
    {
        public int SupplierProductId { get; set; }
        public int SupplierId { get; set; }
        public int ProductId { get; set; }
        public decimal ProductCost { get; set; }
        public short Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }

    }
}
