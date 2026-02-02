using SIMAPI.Data.Dto;
using SIMAPI.Data.Models;

namespace SIMAPI.Business.Interfaces
{
    public interface ILookUpService
    {
        Task<CommonResponse> GetAreaLookupAsync(GetLookupRequest request);
        Task<CommonResponse> GetShopLookupAsync(int areaId);
        Task<CommonResponse> GetAvailableShopCommissionChequesAsync(int shopId,int userRoleId);
        Task<CommonResponse> GetAvailableShopPhysicalCommissionChequesAsync(int shopId,int userRoleId);
        Task<CommonResponse> GetNetworkLookupAsync();
        Task<CommonResponse> GetUserRoleLookupAsync();
        Task<CommonResponse> GetUserLookupAsync(GetLookupRequest request);
        Task<CommonResponse> GetSupplierLookupAsync();
        Task<CommonResponse> GetSupplierAccountLookupAsync(int supplierId);

        Task<CommonResponse> GetMixAndMatchGroups();
        Task<CommonResponse> GetCategories();
        Task<CommonResponse> GetSubCategories(int categoryId);
        Task<CommonResponse> GetAvailableColours();
        Task<CommonResponse> GetAvailableSizes();
        Task<CommonResponse> GetConfigurationTypes();
        Task<CommonResponse> GetProducts();

        Task<CommonResponse> GetOrderStatusTypes();
        Task<CommonResponse> GetOrderPaymentTypes();
        Task<CommonResponse> GetOrderDeliveryTypes();

    }
}
