﻿using SIMAPI.Data.Dto;
using SIMAPI.Data.Models;
using SIMAPI.Data.Models.OnField;

namespace SIMAPI.Business.Interfaces
{
    public interface IShopService
    {
        Task<CommonResponse> GetByIdAsync(int id);
        Task<CommonResponse> GetByNameAsync(string name,string postCode);
        Task<CommonResponse> GetAllAsync();
        Task<CommonResponse> GetByPagingAsync(GetPagedSearch request);
        Task<CommonResponse> CreateAsync(ShopDto request);
        Task<CommonResponse> UpdateAsync(ShopDto request);
        Task<CommonResponse> DeleteAsync(int id);
        Task<CommonResponse> ShopVisitAsync(ShopVisitRequestmodel request);
        Task<CommonResponse> GetShopVisitHistoryAsync(int shopId);
        Task<CommonResponse> GetShopWalletAmountAsync(int shopId);
        Task<CommonResponse> GetShopWalletHistoryAsync(int shopId, string walletType);
        
    }
}