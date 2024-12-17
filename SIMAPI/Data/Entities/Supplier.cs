namespace SIMAPI.Data.Entities
{
    public partial class Supplier
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public short Status { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
