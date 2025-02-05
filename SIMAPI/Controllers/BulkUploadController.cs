using Microsoft.AspNetCore.Mvc;
using SIMAPI.Business.Helper;
using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using System.IO;

namespace SIMAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BulkUploadController : BaseController
    {
        private readonly IBulkUploadService _service;
        private readonly IConfiguration _configuration;
        public BulkUploadController(IBulkUploadService service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
        }


        [HttpPost("Import")]
        public async Task<IActionResult> Import(BulkUploadDto request)
        {
            var result = await _service.UploadFile(request);
            return Json(result);
        }

    }
}