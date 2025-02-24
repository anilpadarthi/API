using Microsoft.AspNetCore.Mvc;
using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Dto;

namespace SIMAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManagementController : BaseController
    {
        private readonly IManagementService _service;
        private readonly IConfiguration _configuration;
        public ManagementController(IManagementService service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
        }


        [HttpPost("CreateWhatsAppNotificationRequest")]
        public async Task<IActionResult> CreateWhatsAppNotificationRequest(WhatsAppRequestDto request)
        {
            var result = await _service.CreateWhatsAppNotificationRequestAsync(request);
            return Json(result);
        }

       



    }
}