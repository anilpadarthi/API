using AutoMapper;
using SIMAPI.Business.Helper;
using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Models;
using SIMAPI.Repository.Interfaces;
using System.Net;
using System.Text.Json;

namespace SIMAPI.Business.Services
{
    public class OnFieldService : IOnFieldService
    {
        private readonly IOnFieldRepository _onFieldRepository;
        private readonly IMapper _mapper;
        public OnFieldService(IOnFieldRepository onFieldRepository, IMapper mapper)
        {
            _onFieldRepository = onFieldRepository;
            _mapper = mapper;
        }

        public async Task<CommonResponse> OnFieldCommissionListAsync(GetReportRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _onFieldRepository.OnFieldCommissionListAsync(request);
                if (result != null)
                {
                    response = Utility.CreateResponse(result, HttpStatusCode.OK);
                }
                else
                {
                    response = Utility.CreateResponse("report does not exist", HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                var json = JsonSerializer.Serialize(request);
                response = await response.HandleException(ex, _onFieldRepository);
            }
            return response;
        }

        public async Task<CommonResponse> OnFieldActivationListAsync(GetReportRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _onFieldRepository.OnFieldActivationListAsync(request);
                if (result != null)
                {
                    response = Utility.CreateResponse(result, HttpStatusCode.OK);
                }
                else
                {
                    response = Utility.CreateResponse("report does not exist", HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                var json = JsonSerializer.Serialize(request);
                response = await response.HandleException(ex, _onFieldRepository,json);
            }
            return response;
        }

        public async Task<CommonResponse> OnFieldGivenVSActivationListync(GetReportRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _onFieldRepository.OnFieldGivenVSActivationListync(request);
                if (result != null)
                {
                    response = Utility.CreateResponse(result, HttpStatusCode.OK);
                }
                else
                {
                    response = Utility.CreateResponse("report does not exist", HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                response = await response.HandleException(ex, _onFieldRepository);
            }
            return response;
        }

        public async Task<CommonResponse> OnFieldSimConversionListAsync(GetReportRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _onFieldRepository.OnFieldSimConversionListAsync(request);
                if (result != null)
                {
                    response = Utility.CreateResponse(result, HttpStatusCode.OK);
                }
                else
                {
                    response = Utility.CreateResponse("report does not exist", HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                response = await response.HandleException(ex, _onFieldRepository);
            }
            return response;
        }

        public async Task<CommonResponse> OnFieldShopVisitHistoryAsync(int shopId)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _onFieldRepository.OnFieldShopVisitHistoryAsync(shopId);
                if (result != null)
                {
                    response = Utility.CreateResponse(result, HttpStatusCode.OK);
                }
                else
                {
                    response = Utility.CreateResponse("report does not exist", HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                response = await response.HandleException(ex, _onFieldRepository);
            }
            return response;
        }

        public async Task<CommonResponse> OnFieildCommissionWalletAmountsAsync(int shopId)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _onFieldRepository.OnFieildCommissionWalletAmountsAsync(shopId);
                if (result != null)
                {
                    response = Utility.CreateResponse(result, HttpStatusCode.OK);
                }
                else
                {
                    response = Utility.CreateResponse("report does not exist", HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                response = await response.HandleException(ex, _onFieldRepository);
            }
            return response;
        }

        public async Task<CommonResponse> OnFieldCommissionWalletHistoryAsync(int shopId, string walletType)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _onFieldRepository.OnFieldCommissionWalletHistoryAsync(shopId, walletType);
                if (result != null)
                {
                    response = Utility.CreateResponse(result, HttpStatusCode.OK);
                }
                else
                {
                    response = Utility.CreateResponse("report does not exist", HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                response = await response.HandleException(ex, _onFieldRepository);
            }
            return response;
        }

        public async Task<decimal> OutstandingBalanceAsync(int shopId)
        {
            var result = await _onFieldRepository.OutstandingBalanceAsync(shopId);
            return result;
        }
    }
}
