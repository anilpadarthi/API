using Microsoft.AspNetCore.Mvc;
using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Dto;

namespace SIMAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DownloadController : BaseController
    {
        private readonly IDownloadService _service;
        private readonly IConfiguration _configuration;
        public DownloadController(IDownloadService service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
        }


        [HttpPost("DownloadInstantActivationList")]
        public async Task<IActionResult> DownloadInstantActivationList(GetReportRequest request)
        {
            request.reportType = "Download";
            var stream = await _service.DownloadInstantActivationListAsync(request);
            return File(stream, "application/octet-stream");
        }

        [HttpPost("DownloadDailyActivationList")]
        public async Task<IActionResult> DownloadDailyActivationList(GetReportRequest request)
        {
            request.loggedInUserId = GetUserId;
            request.userRole = GetUser.userRole.RoleName;
            request.userRoleId = GetUser.userRole.UserRoleId;
            var stream = await _service.DownloadDailyActivtionsAsync(request);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Sales.xlsx");
        }

        [HttpPost("DownloadActivtionAnalysisReport")]
        public async Task<IActionResult> DownloadActivtionAnalysisReportAsync(GetReportRequest request)
        {
            request.loggedInUserId = GetUserId;
            request.userRole = GetUser.userRole.RoleName;
            request.userRoleId = GetUser.userRole.UserRoleId;
            var stream = await _service.DownloadActivtionAnalysisReportAsync(request);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Analysis.xlsx");
        }

        [HttpPost("DownloadCommissionStatementList")]
        public async Task<IActionResult> DownloadCommissionStatementList(GetReportRequest request)
        {
            request.reportType = "Download";
            var stream = await _service.DownloadInstantActivationListAsync(request);
            return File(stream, "application/octet-stream");
        }

        [HttpPost("DownloadVatCommissionStatementList")]
        public async Task<IActionResult> DownloadVatCommissionStatementList(GetReportRequest request)
        {
            request.reportType = "Download";
            var stream = await _service.DownloadInstantActivationListAsync(request);
            return File(stream, "application/octet-stream");
        }


    }
}