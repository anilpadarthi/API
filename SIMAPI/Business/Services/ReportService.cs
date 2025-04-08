using AutoMapper;
using SIMAPI.Business.Enums;
using SIMAPI.Business.Helper;
using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Models;
using SIMAPI.Repository.Interfaces;
using System.Net;

namespace SIMAPI.Business.Services
{
    public class ReportService : IReportService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IMapper _mapper;
        public ReportService(IReportRepository reportRepository, IMapper mapper)
        {
            _reportRepository = reportRepository;
            _mapper = mapper;
        }

        public async Task<CommonResponse> GetMonthlyActivationsAsync(GetReportRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                if (request.userId.HasValue && request.areaId.HasValue && request.shopId.HasValue)
                {
                    request.filterMode = "By Shop";
                    request.filterId = request.shopId;
                }

                else if (request.userId.HasValue && request.areaId.HasValue)
                {
                    request.filterMode = "By Area";
                    request.filterId = request.areaId;
                }

                else if (request.userId.HasValue || request.userRoleId == (int)EnumUserRole.Agent)
                {
                    request.filterMode = "By Agent";
                    request.filterType = "Agent";
                    request.filterId = request.userRoleId == (int)EnumUserRole.Agent ? request.loggedInUserId : request.userId;
                }

                else if (request.managerId.HasValue || request.userRoleId == (int)EnumUserRole.Manager)
                {
                    request.filterMode = "All";
                    request.filterType = "Manager";
                    request.filterId = request.userRoleId == (int)EnumUserRole.Manager ? request.loggedInUserId : request.managerId;
                }
                else
                {
                    request.filterMode = "All";
                }


                var result = await _reportRepository.GetMonthlyActivationsAsync(request);
                if (result != null)
                {
                    response = Utility.CreateResponse(result, HttpStatusCode.OK);
                }
                else
                {
                    response = Utility.CreateResponse("not found", HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex);
            }
            return response;
        }

        public async Task<CommonResponse> GetMonthlyHistoryActivationsAsync(GetReportRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                if (request.userId.HasValue && request.areaId.HasValue && request.shopId.HasValue)
                {
                    request.filterMode = "By Shop";
                    request.filterId = request.shopId;
                }

                else if (request.userId.HasValue && request.areaId.HasValue)
                {
                    request.filterMode = "By Area";
                    request.filterId = request.areaId;
                }

                else if (request.userId.HasValue || request.userRoleId == (int)EnumUserRole.Agent)
                {
                    request.filterMode = "By Agent";
                    request.filterId = request.userRoleId == (int)EnumUserRole.Agent ? request.loggedInUserId : request.userId;
                }
                else if (request.managerId.HasValue || request.userRoleId == (int)EnumUserRole.Manager)
                {
                    request.filterMode = "All";
                    request.filterType = "Manager";
                    request.filterId = request.userRoleId == (int)EnumUserRole.Manager ? request.loggedInUserId : request.managerId;
                }
                else
                {
                    request.filterMode = "All";
                }

                var result = await _reportRepository.GetMonthlyHistoryActivationsAsync(request);
                if (result != null)
                {
                    response = Utility.CreateResponse(result, HttpStatusCode.OK);
                }
                else
                {
                    response = Utility.CreateResponse("History activation report does not exist", HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex);
            }
            return response;
        }

        public async Task<CommonResponse> GetDailyGivenCountAsync(GetReportRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _reportRepository.GetDailyGivenCountAsync(request);
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
                response = response.HandleException(ex);
            }
            return response;
        }


        public async Task<CommonResponse> GetNetworkUsageReportAsync(GetReportRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _reportRepository.GetNetworkUsageReportAsync(request);
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
                response = response.HandleException(ex);
            }
            return response;
        }

        public async Task<CommonResponse> GetKPITargetReportAsync(GetReportRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                request.filterUserRoleId = request.filterId.HasValue ? (int)EnumUserRole.Manager : 0;

                var result = await _reportRepository.GetKPITargetReportAsync(request);
                if (result != null)
                {
                    response = Utility.CreateResponse(result, HttpStatusCode.OK);
                }
                else
                {
                    response = Utility.CreateResponse("Area wise report does not exist", HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex);
            }
            return response;
        }




        public async Task<CommonResponse> GetMonthlyUserActivationsAsync(GetReportRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _reportRepository.GetMonthlyUserActivationsAsync(request);
                if (result != null)
                {
                    response = Utility.CreateResponse(result, HttpStatusCode.OK);
                }
                else
                {
                    response = Utility.CreateResponse("Area wise report does not exist", HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex);
            }
            return response;
        }

        public async Task<CommonResponse> GetMonthlyAreaActivationsAsync(GetReportRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _reportRepository.GetMonthlyAreaActivationsAsync(request);
                if (result != null)
                {
                    response = Utility.CreateResponse(result, HttpStatusCode.OK);
                }
                else
                {
                    response = Utility.CreateResponse("Area wise report does not exist", HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex);
            }
            return response;
        }

        public async Task<CommonResponse> GetMonthlyShopActivationsAsync(GetReportRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _reportRepository.GetMonthlyShopActivationsAsync(request);
                if (result != null)
                {
                    response = Utility.CreateResponse(result, HttpStatusCode.OK);
                }
                else
                {
                    response = Utility.CreateResponse("Area wise report does not exist", HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex);
            }
            return response;
        }



        public async Task<CommonResponse> GetInstantActivationReportAsync(GetReportRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                if (request.reportType == "Instant")
                {
                    var result = await _reportRepository.GetInstantActivationReportAsync(request);
                    response = Utility.CreateResponse(result, HttpStatusCode.OK);
                }
                else if (request.reportType == "Agent")
                {
                    var result = await _reportRepository.GetInstantActivationReportAsync(request);
                    response = Utility.CreateResponse(result, HttpStatusCode.OK);
                }
                else if (request.reportType == "Shop")
                {
                    var result = await _reportRepository.GetInstantActivationReportAsync(request);
                    response = Utility.CreateResponse(result, HttpStatusCode.OK);
                }
                else if (request.reportType == "Download")
                {
                    var result = await _reportRepository.GetInstantActivationReportAsync(request);
                    response = Utility.CreateResponse(result, HttpStatusCode.OK);
                }

            }
            catch (Exception ex)
            {
                response = response.HandleException(ex);
            }
            return response;
        }



        public async Task<CommonResponse> GetSalaryReportAsync(GetReportRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _reportRepository.GetSalaryReportAsync(request);
                if (result != null)
                {
                    response = Utility.CreateResponse(result, HttpStatusCode.OK);
                }
                else
                {
                    response = Utility.CreateResponse("Area wise report does not exist", HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex);
            }
            return response;
        }

        public async Task<CommonResponse> GetSimAllocationReportAsync(GetReportRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _reportRepository.GetSimAllocationReportAsync(request);
                if (result != null)
                {
                    response = Utility.CreateResponse(result, HttpStatusCode.OK);
                }
                else
                {
                    response = Utility.CreateResponse("Area wise report does not exist", HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex);
            }
            return response;
        }

        public async Task<CommonResponse> GetAccessoriesOutstandingReportsAsync(GetReportRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _reportRepository.GetAccessoriesOutstandingReportsAsync(request);
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
                response = response.HandleException(ex);
            }
            return response;
        }

        public async Task<CommonResponse> GetMonthlyAccessoriesReportAsync(GetReportRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                if (request.filterId != null)
                {
                    var result = await _reportRepository.GetDetailsAccessoriesReportAsync(request);
                    if (result != null)
                    {
                        response = Utility.CreateResponse(result, HttpStatusCode.OK);
                    }
                    else
                    {
                        response = Utility.CreateResponse("report does not exist", HttpStatusCode.NotFound);
                    }
                }
                else
                {
                    var result = await _reportRepository.GetMonthlyAccessoriesReportAsync(request);
                    if (result != null)
                    {
                        response = Utility.CreateResponse(result, HttpStatusCode.OK);
                    }
                    else
                    {
                        response = Utility.CreateResponse("report does not exist", HttpStatusCode.NotFound);
                    }
                }
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex);
            }
            return response;
        }
    }
}
