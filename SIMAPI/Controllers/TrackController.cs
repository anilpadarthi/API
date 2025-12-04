using Microsoft.AspNetCore.Mvc;
using SIMAPI.Business.Helper;
using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Models.Export;

namespace SIMAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrackController : BaseController
    {
        private readonly ITrackService _service;
        private readonly IConfiguration _configuration;
        public TrackController(ITrackService service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
        }


        [HttpPost("GetAreasVisitedReport")]
        public async Task<IActionResult> GetAreasVisitedReport(GetReportRequest request)
        {
            request.userId = GetUserId;
            request.userRole = GetUser.userRole.RoleName;
            request.userRoleId = GetUser.userRole.UserRoleId;
            var result = await _service.GetAreasVisitedReportAsync(request);
            return Json(result);
        }

        [HttpPost("GetShopsVisitedReport")]
        public async Task<IActionResult> GetShopsVisitedReport(GetReportRequest request)
        {
            request.userId = GetUserId;
            request.userRole = GetUser.userRole.RoleName;
            request.userRoleId = GetUser.userRole.UserRoleId;
            var result = await _service.GetShopsVisitedReportAsync(request);
            return Json(result);
        }

        [HttpPost("GetShopsSimsGivenReport")]
        public async Task<IActionResult> GetShopsSimsGivenReport(GetReportRequest request)
        {
            request.userId = GetUserId;
            request.userRole = GetUser.userRole.RoleName;
            request.userRoleId = GetUser.userRole.UserRoleId;
            var result = await _service.GetShopsSimsGivenReportAsync(request);
            return Json(result);
        }

        [HttpPost("GetTrackReport")]
        public async Task<IActionResult> GetTrackReport(GetReportRequest request)
        {
            request.userId = GetUserId;
            request.userRole = GetUser.userRole.RoleName;
            request.userRoleId = GetUser.userRole.UserRoleId;
            var result = await _service.GetTrackReportAsync(request);
            return Json(result);
        }

        [HttpPost("GetUserTrackDataReport")]
        public async Task<IActionResult> GetUserTrackDataReport(GetReportRequest request)
        {
            request.userId = GetUserId;
            request.userRole = GetUser.userRole.RoleName;
            request.userRoleId = GetUser.userRole.UserRoleId;
            var result = await _service.GetUserTrackDataReportAsync(request);
            return Json(result);
        }

       

        [HttpPost("GetLatLongReport")]
        public async Task<IActionResult> GetLatLongReport(GetReportRequest request)
        {
            request.userId = GetUserId;
            request.userRoleId = GetUser.userRole.UserRoleId;
            request.filterType = GetUser.userRole.RoleName;
            request.filterId = GetUser.userRole.UserRoleId;
            var result = await _service.GetLatLongReportAsync(request);
            return Json(result);
        }

        [HttpPost("LogUserTrackAsync")]
        public async Task<IActionResult> LogUserTrackAsync(UserTrackDto request)
        {
            var result = await _service.LogUserTrackAsync(request);
            return Json(result);
        }

        [HttpGet("DownloadAttendace")]
        public async Task<IActionResult> DownloadAttendace(string date)
        {
            var result = await _service.DownloadAttendaceAsync(date);
            return Json(result);
        }
    }
}