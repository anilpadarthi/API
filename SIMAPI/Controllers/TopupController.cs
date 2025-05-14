using Microsoft.AspNetCore.Mvc;
using SIMAPI.Business.Interfaces;

namespace SIMAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TopupController : BaseController
    {
        private readonly ITopupService _service;
        private readonly ITopupWalletService _topupWalletService;
        public TopupController(ITopupService service, ITopupWalletService topupWalletService)
        {
            _service = service;
            _topupWalletService = topupWalletService;
        }

        [HttpGet("ValidateIMEI")]
        public async Task<IActionResult> ValidateIMEI(string phoneNo, string shopId, string topupAmount)
        {
            var result = await _service.ValidateIMEI(phoneNo, shopId, topupAmount);
            return Json(result);
        }

        [HttpGet("SaveTopup")]
        public async Task<IActionResult> SaveTopup(string phoneNo, string shopId, string topupAmount)
        {
            var result = await _service.SaveTopup(phoneNo, shopId, topupAmount);
            return Json(result);
        }        

    }

   
}