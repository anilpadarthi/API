using SIMAPI.Data.Entities;

namespace SIMAPI.Data.Dto
{
    public class SupplierDto
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public short Status { get; set; }
        public int? CreatedBy { get; set; }
        public List<SupplierAccount>? SupplierAccounts { get; set; }
        public List<SupplierProduct>? SupplierProducts { get; set; }
    }

    public class SupplierTransactionDto
    {
        public int SupplierId { get; set; }
        public string TransactionType { get; set; }
        public string ReferenceNumber { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
    }
}
