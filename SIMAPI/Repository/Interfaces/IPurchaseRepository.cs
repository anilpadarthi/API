using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;

namespace SIMAPI.Repository.Interfaces
{
    public interface IPurchaseRepository: IRepository
    {

        Task<PurchaseInvoice> GetPurchaseInvoiceDetailsByIdAsync(int purchaseInvoiceId);
        Task<IEnumerable<PurchaseInvoice>> GetInvoiceListPagingAsync(GetPagedSearch request);
        Task<int> GetTotalInvoicesCountAsync(GetPagedSearch request);
    }
}
