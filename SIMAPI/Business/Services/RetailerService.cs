using AutoMapper;
using SIMAPI.Business.Helper;
using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Models;
using SIMAPI.Repository.Interfaces;
using System.Net;


namespace SIMAPI.Business.Services
{
    public class RetailerService : IRetailerService
    {
        private readonly IRetailerRepository _retailerRepository;
        private readonly IMapper _mapper;
        public RetailerService(IRetailerRepository retailerRepository, IMapper mapper)
        {
            _retailerRepository = retailerRepository;
            _mapper = mapper;
        }

        public async Task<CommonResponse> GetActvationsAsync(GetReportRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _retailerRepository.GetActivationDetaiListAsync(request);
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
                response = response.HandleException(ex, _retailerRepository);
            }
            return response;
        }

        public async Task<CommonResponse> GetActivationDetaiListAsync(GetReportRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _retailerRepository.GetActivationDetaiListAsync(request);
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
                response = response.HandleException(ex, _retailerRepository);
            }
            return response;
        }

        public async Task<CommonResponse> GetSimGivenAsync(GetReportRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _retailerRepository.GetSimGivenAsync(request);
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
                response = response.HandleException(ex, _retailerRepository);
            }
            return response;
        }

        public async Task<CommonResponse> GetStockVsConnectionsAsync(GetReportRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _retailerRepository.GetStockVsConnectionsAsync(request);
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
                response = response.HandleException(ex, _retailerRepository);
            }
            return response;
        }

        public async Task<CommonResponse> GetRetailerCommissionListAsync(GetReportRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _retailerRepository.GetRetailerCommissionListAsync(request);
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
                response = response.HandleException(ex, _retailerRepository);
            }
            return response;
        }
    }
}
