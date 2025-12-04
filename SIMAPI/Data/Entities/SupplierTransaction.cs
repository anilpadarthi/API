namespace SIMAPI.Data.Entities
{
    public partial class SupplierTransaction
    {
        public int SupplierTransactionId { get; set; }
        public int SupplierId { get; set; }
        public string? TransactionType { get; set; }
        public string? ReferenceNumber { get; set; }
        public decimal? Amount { get; set; }
        public short IsActive { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
