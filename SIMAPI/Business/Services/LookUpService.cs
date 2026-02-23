using SIMAPI.Business.Enums;
using SIMAPI.Business.Helper;
using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Models;
using SIMAPI.Repository.Interfaces;
using SIMAPI.Repository.Repositories;
using System.Net;

namespace SIMAPI.Business.Services
{
    public class LookUpService : ILookUpService
    {
        private readonly ILookUpRepository _lookupRepository;
        public LookUpService(ILookUpRepository lookupRepository)
        {
            _lookupRepository = lookupRepository;
        }

        public async Task<CommonResponse> GetAreaLookupAsync(GetLookupRequest request)
        {
            var result = await _lookupRepository.GetAreaLookup(request);
            return Utility.CreateResponse(result, HttpStatusCode.OK);
        }

        public async Task<CommonResponse> GetShopLookupAsync(int areaId)
        {
            var result = await _lookupRepository.GetShopLookup(areaId);
            return Utility.CreateResponse(result, HttpStatusCode.OK);
        }

        public async Task<CommonResponse> GetAvailableShopCommissionChequesAsync(int shopId, int userRoleId)
        {
            var result = await _lookupRepository.GetAvailableShopCommissionChequesAsync(shopId);
            if (userRoleId == (int)EnumUserRole.Admin || userRoleId == (int)EnumUserRole.SuperAdmin)
            {
                return Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            else
            {
                //result = result.Where(w => Convert.ToDouble(w.Name) >= 12);
                return Utility.CreateResponse(result, HttpStatusCode.OK);
            }
        }

        public async Task<CommonResponse> GetAvailableShopPhysicalCommissionChequesAsync(int shopId, int userRoleId)
        {
            var result = await _lookupRepository.GetAvailableShopPhysicalCommissionChequesAsync(shopId);
            if (userRoleId == (int)EnumUserRole.Admin || userRoleId == (int)EnumUserRole.SuperAdmin)
            {
                return Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            else
            {
                //result = result.Where(w => Convert.ToDouble(w.Name) >= 12);
                return Utility.CreateResponse(result, HttpStatusCode.OK);
            }
        }

        public async Task<CommonResponse> GetNetworkLookupAsync()
        {
            var result = await _lookupRepository.GetNetworkLookup();
            return Utility.CreateResponse(result, HttpStatusCode.OK);
        }

        public async Task<CommonResponse> GetUserLookupAsync(GetLookupRequest request)
        {
            var result = await _lookupRepository.GetUserLookup(request);
            return Utility.CreateResponse(result, HttpStatusCode.OK);
        }

        public async Task<CommonResponse> GetUserRoleLookupAsync()
        {
            var result = await _lookupRepository.GetUserRoleLookupAsync();
            return Utility.CreateResponse(result, HttpStatusCode.OK);
        }

        public async Task<CommonResponse> GetSupplierLookupAsync()
        {
            var result = await _lookupRepository.GetSupplierLookupAsync();
            return Utility.CreateResponse(result, HttpStatusCode.OK);
        }

        public async Task<CommonResponse> GetSupplierAccountLookupAsync(int supplierId)
        {
            var result = await _lookupRepository.GetSupplierAccountLookupAsync(supplierId);
            return Utility.CreateResponse(result, HttpStatusCode.OK);
        }

        public async Task<CommonResponse> GetMixAndMatchGroups()
        {
            var list = await _lookupRepository.GetMixAndMatchGroups();
            return Utility.CreateResponse(list, HttpStatusCode.OK);
        }

        public async Task<CommonResponse> GetCategories()
        {
            var list = await _lookupRepository.GetCategories();
            return Utility.CreateResponse(list, HttpStatusCode.OK);
        }

        public async Task<CommonResponse> GetSubCategories(int categoryId)
        {
            var list = await _lookupRepository.GetSubCategories(categoryId);
            return Utility.CreateResponse(list, HttpStatusCode.OK);
        }

        public async Task<CommonResponse> GetAvailableColours()
        {
            var list = await _lookupRepository.GetAvailableColours();
            return Utility.CreateResponse(list, HttpStatusCode.OK);
        }

        public async Task<CommonResponse> GetAvailableSizes()
        {
            var list = await _lookupRepository.GetAvailableSizes();
            return Utility.CreateResponse(list, HttpStatusCode.OK);
        }

        public async Task<CommonResponse> GetConfigurationTypes()
        {
            var list = await _lookupRepository.GetConfigurationTypes();
            return Utility.CreateResponse(list, HttpStatusCode.OK);
        }

        public async Task<CommonResponse> GetProducts()
        {
            var list = await _lookupRepository.GetProducts();
            return Utility.CreateResponse(list, HttpStatusCode.OK);
        }

        public async Task<CommonResponse> GetOrderStatusTypes()
        {
            var list = await _lookupRepository.GetOrderStatusTypes();
            return Utility.CreateResponse(list, HttpStatusCode.OK);
        }


        public async Task<CommonResponse> GetOrderPaymentTypes()
        {
            var list = await _lookupRepository.GetOrderPaymentTypes();
            return Utility.CreateResponse(list, HttpStatusCode.OK);
        }


        public async Task<CommonResponse> GetOrderDeliveryTypes()
        {
            var list = await _lookupRepository.GetOrderDeliveryTypes();
            return Utility.CreateResponse(list, HttpStatusCode.OK);
        }
    }
}
