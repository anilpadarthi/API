using Microsoft.AspNetCore.Mvc;
using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Dto;

namespace SIMAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : BaseController
    {
        private readonly IDashboardService _service;
        private readonly IConfiguration _configuration;
        public DashboardController(IDashboardService service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
        }


        [HttpPost("GetAreaWiseActivations")]
        public async Task<IActionResult> GetAreaWiseActivations(GetReportRequest request)
        {
            request.loggedInUserRole = GetUser.userRole.RoleName;
            var result = await _service.GetAreaWiseActivationsAsync(request);
            return Json(result);
        }

        [HttpPost("GetNetworkWiseActivations")]
        public async Task<IActionResult> GetNetworkWiseActivations(GetReportRequest request)
        {
            request.loggedInUserId = GetUserId;
            request.loggedInUserRole = GetUser.userRole.RoleName;
            request.userRoleId = GetUser.userRole.UserRoleId;
            var result = await _service.GetNetworkWiseActivationsAsync(request);
            return Json(result);
        }

        [HttpPost("GetSimAllocationReport")]
        public async Task<IActionResult> GetSimAllocationReport(GetReportRequest request)
        {
            request.loggedInUserId = GetUserId;
            request.userRole = GetUser.userRole.RoleName;
            request.userRoleId = GetUser.userRole.UserRoleId;
            var result = await _service.GetSimAllocationReportAsync(request);
            return Json(result);
        }


        [HttpPost("GetUserWiseActivations")]
        public async Task<IActionResult> GetUserWiseActivations(GetReportRequest request)
        {
            request.loggedInUserId = GetUserId;
            request.userRole = GetUser.userRole.RoleName;
            request.userRoleId = GetUser.userRole.UserRoleId;
            var result = await _service.GetUserWiseActivationsAsync(request);
            return Json(result);
        }

        [HttpPost("GetUserWiseAccessoriesSales")]
        public async Task<IActionResult> GetUserWiseAccessoriesSales(GetReportRequest request)
        {
            request.loggedInUserId = GetUserId;
            request.userRole = GetUser.userRole.RoleName;
            request.userRoleId = GetUser.userRole.UserRoleId;
            var result = await _service.GetUserWiseAccessoriesSalesAsync(request);
            return Json(result);
        }

        [HttpPost("GetUserWiseKPIReport")]
        public async Task<IActionResult> GetUserWiseKPIReport(GetReportRequest request)
        {
            var result = await _service.GetUserWiseKPIReportAsync(request);
            return Json(result);
        }

        [HttpPost("GetUserWiseAccessoriesKPIReport")]
        public async Task<IActionResult> GetUserWiseAccessoriesKPIReport(GetReportRequest request)
        {
            var result = await _service.GetUserWiseAccessoriesKPIReportAsync(request);
            return Json(result);
        }

        [HttpPost("GetDahboardMetrics")]
        public async Task<IActionResult> GetDahboardMetrics(GetReportRequest request)
        {
            var result = await _service.GetDahboardMetricsAsync(request);
            return Json(result);
        }

        [HttpPost("GetDahboardChartActivationMetrics")]
        public async Task<IActionResult> GetDahboardChartActivationMetrics(GetReportRequest request)
        {
            request.userId = GetUserId;
            request.userRoleId = GetUser.userRoleId;
            request.userRole = GetUser.userRole.RoleName;
            var result = await _service.GetDahboardChartActivationMetricsAsync(request);
            return Json(result);
        }

        [HttpPost("GetDahboardAccessoriesMetrics")]
        public async Task<IActionResult> GetDahboardAccessoriesMetrics(GetReportRequest request)
        {
            var result = await _service.GetDahboardAccessoriesMetricsAsync(request);
            return Json(result);
        }

    }
}