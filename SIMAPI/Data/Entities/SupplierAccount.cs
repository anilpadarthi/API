namespace SIMAPI.Data.Entities
{
    public partial class SupplierAccount
    {
        public int SupplierAccountId { get; set; }
        public int SupplierId { get; set; }
        public string AccountName { get; set; }
        public string AccountNumber { get; set; }
        public short Status { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

    }
}
