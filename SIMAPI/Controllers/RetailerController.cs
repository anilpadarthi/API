using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SIMAPI.Business.Helper;
using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Models.Export;
using SIMAPI.Data.Models.OnField;

namespace SIMAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RetailerController : BaseController
    {
        private readonly IRetailerService _service;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public RetailerController(IRetailerService service, IConfiguration configuration, IMapper mapper)
        {
            _service = service;
            _configuration = configuration;
            _mapper = mapper;

        }


        [HttpPost("GetActvations")]
        public async Task<IActionResult> GetActvations(GetReportRequest request)
        {
            request.loggedInUserId = GetUserId;
            request.userRole = GetUser.UserRole.RoleName;
            request.userRoleId = GetUser.UserRole.UserRoleId;
            var result = await _service.GetActvationsAsync(request);
            return Json(result);
        }

        [HttpPost("GetCommissions")]
        public async Task<IActionResult> GetCommissions(GetReportRequest request)
        {
            request.loggedInUserId = GetUserId;
            request.userRole = GetUser.UserRole.RoleName;
            request.userRoleId = GetUser.UserRole.UserRoleId;
            var result = await _service.GetCommissionsAsync(request);
            return Json(result);
        }

        [HttpPost("GetStockVsConnections")]
        public async Task<IActionResult> GetStockVsConnections(GetReportRequest request)
        {
            request.loggedInUserId = GetUserId;
            request.userRole = GetUser.UserRole.RoleName;
            request.userRoleId = GetUser.UserRole.UserRoleId;
            var result = await _service.GetStockVsConnectionsAsync(request);
            return Json(result);
        }



    }
}