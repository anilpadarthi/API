using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models;
using SIMAPI.Data.Models.OnField;

namespace SIMAPI.Repository.Interfaces
{
    public interface IShopRepository : IRepository
    {
        Task<Shop> GetShopByIdAsync(int shopId);
        Task<ShopDetails> GetShopDetailsAsync(int shopId);
        Task<Shop> GetShopByNameAsync(string name,string postCode);
        Task<IEnumerable<ShopContact>> GetShopContactsAsync(int shopId);
        Task<ShopAgreement> GetShopAgreementAsync(int shopId);
        Task<IEnumerable<Shop>> GetAllShopsAsync();
        Task<IEnumerable<Shop>> GetShopsByPagingAsync(GetPagedSearch request);
        Task<int> GetTotalShopsCountAsync(GetPagedSearch request);
        Task<bool> ShopVisitAsync(ShopVisitRequestmodel request);
        Task<IEnumerable<ShopVisitHistoryModel>> GetShopVisitHistoryAsync(int shopId);       
        Task<IEnumerable<ShopAgreementHistoryModel>> GetShopAgreementHistoryAsync(int shopId);       
        Task<ShopWalletAmountModel> GetShopWalletAmountAsync(int shopId);
        Task<ShopAddressDetails?> GetShopAddressDetailsAsync(int shopId);
        Task<IEnumerable<ShopWalletHistoryModel>> GetShopWalletHistoryAsync(int shopId,string walletType);
    }
}
