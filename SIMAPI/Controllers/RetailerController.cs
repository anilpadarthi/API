using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Dto;

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
            request.filterId = GetUserId;
            var result = await _service.GetActvationsAsync(request);
            return Json(result);
        }

        [HttpPost("GetSimGiven")]
        public async Task<IActionResult> GetSimGiven(GetReportRequest request)
        {
            request.filterId = GetUserId;
            var result = await _service.GetSimGivenAsync(request);
            return Json(result);
        }

        [HttpPost("GetRetailerCommissionList")]
        public async Task<IActionResult> GetRetailerCommissionList(GetReportRequest request)
        {
            request.filterId = GetUserId;
            var result = await _service.GetRetailerCommissionListAsync(request);
            return Json(result);
        }

        [HttpPost("GetStockVsConnections")]
        public async Task<IActionResult> GetStockVsConnections(GetReportRequest request)
        {
            request.loggedInUserId = GetUserId;
            var result = await _service.GetStockVsConnectionsAsync(request);
            return Json(result);
        }



    }
}