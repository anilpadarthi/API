using SIMAPI.Data.Entities;

namespace SIMAPI.Data.Models
{
    public class SupplierDetails
    {
        public Supplier supplier { get; set; }
        public IEnumerable<SupplierAccount> supplierAccounts { get; set; }
        public IEnumerable<SupplierProduct> supplierProducts { get; set; }
    }
}
