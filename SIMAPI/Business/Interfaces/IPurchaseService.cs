using SIMAPI.Data.Dto;
using SIMAPI.Data.Models;

namespace SIMAPI.Business.Interfaces
{
    public interface IPurchaseService
    {
        Task<CommonResponse> CreatePurchaseAsync(PurchaseInvoiceCreateDto request);
        Task<CommonResponse> UpdatePurchaseAsync(PurchaseInvoiceCreateDto request);

        Task<CommonResponse> GetByIdAsync(int id);
        Task<CommonResponse> GetItemsAsync(int id);
        Task<CommonResponse> GetByPagingAsync(GetPagedSearch request);


    }
}
