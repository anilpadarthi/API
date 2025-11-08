using Microsoft.AspNetCore.Mvc;
using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Dto;

namespace SIMAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController : BaseController
    {
        private readonly IReportService _service;
        private readonly IConfiguration _configuration;
        public ReportController(IReportService service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
        }

        [HttpPost("GetMonthlyActivations")]
        public async Task<IActionResult> GetMonthlyActivations(GetReportRequest request)
        {
            request.loggedInUserId = GetUserId;
            request.userRole = GetUser.userRole.RoleName;
            request.userRoleId = GetUser.userRole.UserRoleId;
            var result = await _service.GetMonthlyActivationsAsync(request);
            return Json(result);
        }

        [HttpPost("GetMonthlyHistoryActivations")]
        public async Task<IActionResult> GetMonthlyHistoryActivations(GetReportRequest request)
        {
            request.loggedInUserId = GetUserId;
            request.userRole = GetUser.userRole.RoleName;
            request.userRoleId = GetUser.userRole.UserRoleId;
            var result = await _service.GetMonthlyHistoryActivationsAsync(request);
            return Json(result);
        }

        [HttpPost("GetDailyGivenCount")]
        public async Task<IActionResult> GetDailyGivenCount(GetReportRequest request)
        {
            request.loggedInUserId = GetUserId;
            request.userRole = GetUser.userRole.RoleName;
            request.userRoleId = GetUser.userRole.UserRoleId;
            var result = await _service.GetDailyGivenCountAsync(request);
            return Json(result);
        }

        [HttpPost("GetNetworkUsageReport")]
        public async Task<IActionResult> GetNetworkUsageReport(GetReportRequest request)
        {
            request.loggedInUserId = GetUserId;
            request.userRole = GetUser.userRole.RoleName;
            request.userRoleId = GetUser.userRole.UserRoleId;
            var result = await _service.GetNetworkUsageReportAsync(request);
            return Json(result);
        }

        [HttpPost("GetKPITargetReport")]
        public async Task<IActionResult> GetKPITargetReport(GetReportRequest request)
        {
            request.loggedInUserId = GetUserId;
            request.userRole = GetUser.userRole.RoleName;
            request.userRoleId = GetUser.userRole.UserRoleId;
            var result = await _service.GetKPITargetReportAsync(request);
            return Json(result);
        }



        [HttpPost("GetInstantActivationReport")]
        public async Task<IActionResult> GetInstantActivationReport(GetReportRequest request)
        {
            request.reportType = "Instant";
            var result = await _service.GetInstantActivationReportAsync(request);
            return Json(result);
        }

        [HttpPost("GetAgentInstantActivationReport")]
        public async Task<IActionResult> GetAgentInstantActivationReport(GetReportRequest request)
        {
            request.reportType = "Agent";
            var result = await _service.GetInstantActivationReportAsync(request);
            return Json(result);
        }

        [HttpPost("GetShopInstantActivationReport")]
        public async Task<IActionResult> GetShopInstantActivationReport(GetReportRequest request)
        {
            request.reportType = "Shop";
            var result = await _service.GetInstantActivationReportAsync(request);
            return Json(result);
        }



        [HttpPost("GetSalaryReport")]
        public async Task<IActionResult> GetSalaryReport(GetReportRequest request)
        {
            var result = await _service.GetSalaryReportAsync(request);
            return Json(result);
        }

        

        [HttpPost("GetUserReport")]
        public async Task<IActionResult> GetUserReport(GetReportRequest request)
        {
            var result = await _service.GetMonthlyUserActivationsAsync(request);
            return Json(result);
        }

        [HttpPost("GetSupplierReport")]
        public async Task<IActionResult> GetSupplierReport(GetReportRequest request)
        {
            var result = await _service.GetMonthlyUserActivationsAsync(request);
            return Json(result);
        }

        [HttpPost("GetMonthlyAreaActivations")]
        public async Task<IActionResult> GetMonthlyAreaActivations(GetReportRequest request)
        {
            request.userId = GetUserId;
            request.userRole = GetUser.userRole.RoleName;
            var result = await _service.GetMonthlyAreaActivationsAsync(request);
            return Json(result);
        }

        [HttpPost("GetMonthlyShopActivations")]
        public async Task<IActionResult> GetMonthlyShopActivations(GetReportRequest request)
        {
            request.userId = GetUserId;
            request.userRole = GetUser.userRole.RoleName;
            var result = await _service.GetMonthlyShopActivationsAsync(request);
            return Json(result);
        }

        [HttpPost("GetSimAllocationReport")]
        public async Task<IActionResult> GetSimAllocationReport(GetReportRequest request)
        {
            request.userId = GetUserId;
            request.userRole = GetUser.userRole.RoleName;
            request.userRoleId = GetUser.userRole.UserRoleId;
            var result = await _service.GetSimAllocationReportAsync(request);
            return Json(result);
        }

        [HttpPost("GetAccessoriesOutstandingReport")]
        public async Task<IActionResult> GetAccessoriesOutstandingReport(GetReportRequest request)
        {
            request.userId = GetUserId;
            request.userRole = GetUser.userRole.RoleName;
            request.userRoleId = GetUser.userRole.UserRoleId;
            var result = await _service.GetAccessoriesOutstandingReportsAsync(request);
            return Json(result);
        }

        [HttpPost("GetMonthlyAccessoriesReport")]
        public async Task<IActionResult> GetMonthlyAccessoriesReport(GetReportRequest request)
        {
            request.loggedInUserId = GetUserId;
            request.userRole = GetUser.userRole.RoleName;
            request.userRoleId = GetUser.userRole.UserRoleId;
            var result = await _service.GetMonthlyAccessoriesReportAsync(request);
            return Json(result);
        }

        [HttpPost("GetMonthlyAccessoriesCommissionPercentReport")]
        public async Task<IActionResult> GetMonthlyAccessoriesCommissionPercentReport(GetReportRequest request)
        {
            request.loggedInUserId = GetUserId;
            request.userRole = GetUser.userRole.RoleName;
            request.userRoleId = GetUser.userRole.UserRoleId;
            var result = await _service.GetMonthlyAccessoriesCommissionPercentReportAsync(request);
            return Json(result);
        }


        [HttpPost("GetChequeWithdrawnReport")]
        public async Task<IActionResult> GetChequeWithdrawnReport(GetReportRequest request)
        {
            request.loggedInUserId = GetUserId;
            request.userRole = GetUser.userRole.RoleName;
            request.userRoleId = GetUser.userRole.UserRoleId;
            var result = await _service.GetChequeWithdrawnReportsAsync(request);
            return Json(result);
        }

        [HttpGet("GetBankChequeStatus")]
        public async Task<IActionResult> GetBankChequeStatus(string chequeNumber)
        {
            var result = await _service.GetBankChequeStatusAsync(chequeNumber);
            return Json(result);
        }

    }
}