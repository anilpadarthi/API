﻿using SIMAPI.Data.Dto;
using SIMAPI.Data.Models;

namespace SIMAPI.Business.Interfaces
{
    public interface ILookUpService
    {
        Task<CommonResponse> GetAreaLookupAsync(GetLookupRequest request);
        Task<CommonResponse> GetShopLookupAsync(int areaId);
        Task<CommonResponse> GetNetworkLookupAsync();
        Task<CommonResponse> GetUserRoleLookupAsync();
        Task<CommonResponse> GetUserLookupAsync(GetLookupRequest request);
        Task<CommonResponse> GetSupplierLookupAsync();
        Task<CommonResponse> GetSupplierAccountLookupAsync(int supplierId);

    }
}