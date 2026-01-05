using AutoMapper;
using SIMAPI.Business.Helper;
using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Models;
using SIMAPI.Repository.Interfaces;
using System.Net;

namespace SIMAPI.Business.Services
{
    public class DasboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IMapper _mapper;
        public DasboardService(IDashboardRepository simRepository, IMapper mapper)
        {
            _dashboardRepository = simRepository;
            _mapper = mapper;
        }
        public async Task<CommonResponse> GetAreaWiseActivationsAsync(GetReportRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _dashboardRepository.GetAreaWiseActivationsAsync(request);
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
               response = response.HandleException(ex, _dashboardRepository);
            }
            return response;
        }

        public async Task<CommonResponse> GetNetworkWiseActivationsAsync(GetReportRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _dashboardRepository.GetNetworkWiseActivationsAsync(request);
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
               response = response.HandleException(ex, _dashboardRepository);
            }
            return response;
        }

        public async Task<CommonResponse> GetSimAllocationReportAsync(GetReportRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _dashboardRepository.GetSimAllocationReportAsync(request);
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
               response = response.HandleException(ex, _dashboardRepository);
            }
            return response;
        }

        public async Task<CommonResponse> GetUserWiseActivationsAsync(GetReportRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _dashboardRepository.GetUserWiseActivationsAsync(request);
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
               response = response.HandleException(ex, _dashboardRepository);
            }
            return response;
        }

        public async Task<CommonResponse> GetUserWiseKPIReportAsync(GetReportRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _dashboardRepository.GetUserWiseKPIReportAsync(request);
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
               response = response.HandleException(ex, _dashboardRepository);
            }
            return response;
        }

        public async Task<CommonResponse> GetUserWiseAccessoriesKPIReportAsync(GetReportRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _dashboardRepository.GetUserWiseAccessoriesKPIReportAsync(request);
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
                response = response.HandleException(ex, _dashboardRepository);
            }
            return response;
        }

        public async Task<CommonResponse> GetDahboardChartActivationMetricsAsync(GetReportRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _dashboardRepository.GetDahboardChartActivationMetricsAsync(request);
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
               response = response.HandleException(ex, _dashboardRepository);
            }
            return response;
        }

        public async Task<CommonResponse> GetDahboardMetricsAsync(GetReportRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _dashboardRepository.GetDahboardMetricsAsync(request);
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
               response = response.HandleException(ex, _dashboardRepository);
            }
            return response;
        }

        public async Task<CommonResponse> GetDahboardAccessoriesMetricsAsync(GetReportRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _dashboardRepository.GetDahboardAccessoriesMetricsAsync(request);
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
                response = response.HandleException(ex, _dashboardRepository);
            }
            return response;
        }
    }
}
