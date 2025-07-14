using SIMAPI.Data.Dto;
using SIMAPI.Data.Models.OnField;

namespace SIMAPI.Repository.Interfaces
{
    public interface IOnFieldRepository : IRepository
    {
        Task<IEnumerable<OnFieldCommissionModel>> OnFieldCommissionListAsync(GetReportRequest request);
        Task<IEnumerable<OnFieldActivationModel>> OnFieldActivationListAsync(GetReportRequest request);
        Task<List<dynamic>> OnFieldGivenVSActivationListync(GetReportRequest request);
        Task<IEnumerable<ShopVisitHistoryModel>> OnFieldShopVisitHistoryAsync(int shopId);
        Task<ShopWalletAmountModel> OnFieildCommissionWalletAmountsAsync(int shopId);
        Task<IEnumerable<ShopWalletHistoryModel>> OnFieldCommissionWalletHistoryAsync(int shopId,string walletType);
        Task<decimal> OutstandingBalanceAsync(int shopId);
    }
}
