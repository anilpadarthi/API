using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models;

namespace SIMAPI.Repository.Interfaces
{
    public interface ISupplierRepository : IRepository
    {
        Task<Supplier> GetSupplierByIdAsync(int supplierId);
        Task<SupplierDetails> GetSupplierDetailsAsync(int supplierId);
        Task<Supplier> GetSupplierByNameAsync(string name);
        Task<IEnumerable<Supplier>> GetAllSuppliersAsync();
        Task<IEnumerable<SupplierListModel>> GetSuppliersByPagingAsync(GetPagedSearch request);
        Task<int> GetTotalSuppliersCountAsync(GetPagedSearch request);
        Task<List<SupplierAccount>> GetSupplierAccountsByIdAsync(int supplierId);
        Task<SupplierAccount> GetSupplierAccountByIdAsync(int supplierAccountId);
        Task<IEnumerable<SupplierTransaction>> GetSupplierTransactionsAsync(int supplierId);
    }
}
