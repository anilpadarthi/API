using SIMAPI.Data.Dto;
using SIMAPI.Data.Models;

namespace SIMAPI.Business.Interfaces
{
    public interface ISupplierService
    {

        Task<CommonResponse> GetByIdAsync(int id);
        Task<CommonResponse> GetByNameAsync(string name);
        Task<CommonResponse> GetAllAsync();
        Task<CommonResponse> GetByPagingAsync(GetPagedSearch request);
        Task<CommonResponse> CreateTransactionAsync(SupplierTransactionDto request);
        Task<CommonResponse> GetSupplierTransactionsAsync(int supplierId);
        Task<CommonResponse> CreateAsync(SupplierDto request);
        Task<CommonResponse> UpdateAsync(SupplierDto request);
        Task<CommonResponse> DeleteAsync(int id);
        Task<CommonResponse> GetSupplierReportAsync(GetReportRequest request);
        Task<CommonResponse> GetHistoricalSupplierReportAsync(GetReportRequest request);
    }
}
