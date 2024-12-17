using SIMAPI.Data.Models;

namespace SIMAPI.Business.Interfaces
{
    public interface IBankChequeService
    {

        Task<CommonResponse> GetShopCommissionChequeDetailsAsync(int shopId);
        Task<CommonResponse> ReplaceShopCommissionCheque(int shopId);
        Task<CommonResponse> SaveNewShopCommissionCheque(int shopId);
    }
}
