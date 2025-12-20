using SIMAPI.Data.Dto;
using SIMAPI.Data.Models;

namespace SIMAPI.Business.Interfaces
{
    public interface IOnFieldService
    {
        Task<CommonResponse> OnFieldCommissionListAsync(GetReportRequest request);
        Task<CommonResponse> OnFieldActivationListAsync(GetReportRequest request);
        Task<CommonResponse> OnFieldGivenVSActivationListync(GetReportRequest request);
        Task<CommonResponse> OnFieldSimConversionListAsync(GetReportRequest request);
        Task<CommonResponse> OnFieldShopVisitHistoryAsync(int shopId);
        Task<CommonResponse> OnFieildCommissionWalletAmountsAsync(int shopId);
        Task<CommonResponse> OnFieldCommissionWalletHistoryAsync(int shopId, string walletType);
        Task<decimal> OutstandingBalanceAsync(int shopId);

    }
}
