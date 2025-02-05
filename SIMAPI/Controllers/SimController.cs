using Microsoft.AspNetCore.Mvc;
using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Dto;

namespace SIMAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SimController : BaseController
    {
        private readonly ISimService _service;
        private readonly IConfiguration _configuration;
        public SimController(ISimService service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
        }

        [HttpPost("ScanSims")]
        public async Task<IActionResult> ScanSims(GetSimInfoRequest request)
        {
            var result = await _service.ScanSimsAsync(request);
            return Json(result);
        }

        [HttpPost("AllocateSims")]
        public async Task<IActionResult> AllocateSims(GetSimInfoRequest request)
        {
            request.loggedInUserId = GetUserId;
            var result = await _service.AllocateSimsAsync(request);
            return Json(result);
        }

        [HttpPost("DeAllocateSims")]
        public async Task<IActionResult> DeAllocateSims(GetSimInfoRequest request)
        {
            request.loggedInUserId = GetUserId;
            var result = await _service.DeAllocateSimsAsync(request);
            return Json(result);
        }

        [HttpPost("GetSimHistoryDetails")]
        public async Task<IActionResult> GetSimHistoryDetails(GetSimInfoRequest request)
        {
            var result = await _service.GetSimHistoryDetailsAsync(request);
            return Json(result);
        }

    }
}