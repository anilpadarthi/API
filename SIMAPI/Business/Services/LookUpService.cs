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
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _lookupRepository.GetAreaLookup(request);
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response.HandleException(ex);
            }
            return response;
        }

        public async Task<CommonResponse> GetShopLookupAsync(int areaId)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _lookupRepository.GetShopLookup(areaId);
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response.HandleException(ex);
            }
            return response;
        }

        public async Task<CommonResponse> GetAvailableShopCommissionChequesAsync(int shopId)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _lookupRepository.GetAvailableShopCommissionChequesAsync(shopId);
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response.HandleException(ex);
            }
            return response;
        }

        public async Task<CommonResponse> GetNetworkLookupAsync()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _lookupRepository.GetNetworkLookup();
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response.HandleException(ex);
            }
            return response;
        }

        public async Task<CommonResponse> GetUserLookupAsync(GetLookupRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _lookupRepository.GetUserLookup(request);
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response.HandleException(ex);
            }
            return response;
        }

        public async Task<CommonResponse> GetUserRoleLookupAsync()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _lookupRepository.GetUserRoleLookupAsync();
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response.HandleException(ex);
            }
            return response;
        }

        public async Task<CommonResponse> GetSupplierLookupAsync()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _lookupRepository.GetSupplierLookupAsync();
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response.HandleException(ex);
            }
            return response;
        }

        public async Task<CommonResponse> GetSupplierAccountLookupAsync(int supplierId)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _lookupRepository.GetSupplierAccountLookupAsync(supplierId);
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response.HandleException(ex);
            }
            return response;
        }

        public async Task<CommonResponse> GetCategories()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var list = await _lookupRepository.GetCategories();
                response = Utility.CreateResponse(list, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex);
            }
            return response;
        }

        public async Task<CommonResponse> GetSubCategories(int categoryId)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var list = await _lookupRepository.GetSubCategories(categoryId);
                response = Utility.CreateResponse(list, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex);
            }
            return response;
        }

        public async Task<CommonResponse> GetAvailableColours()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var list = await _lookupRepository.GetAvailableColours();
                response = Utility.CreateResponse(list, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex);
            }
            return response;
        }

        public async Task<CommonResponse> GetAvailableSizes()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var list = await _lookupRepository.GetAvailableSizes();
                response = Utility.CreateResponse(list, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex);
            }
            return response;
        }

        public async Task<CommonResponse> GetConfigurationTypes()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var list = await _lookupRepository.GetConfigurationTypes();
                response = Utility.CreateResponse(list, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex);
            }
            return response;
        }

        public async Task<CommonResponse> GetProducts()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var list = await _lookupRepository.GetProducts();
                response = Utility.CreateResponse(list, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex);
            }
            return response;
        }

        public async Task<CommonResponse> GetOrderStatusTypes()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var list = await _lookupRepository.GetOrderStatusTypes();
                response = Utility.CreateResponse(list, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex);
            }
            return response;
        }


        public async Task<CommonResponse> GetOrderPaymentTypes()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var list = await _lookupRepository.GetOrderPaymentTypes();
                response = Utility.CreateResponse(list, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex);
            }
            return response;
        }


        public async Task<CommonResponse> GetOrderDeliveryTypes()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var list = await _lookupRepository.GetOrderDeliveryTypes();
                response = Utility.CreateResponse(list, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex);
            }
            return response;
        }
    }
}
