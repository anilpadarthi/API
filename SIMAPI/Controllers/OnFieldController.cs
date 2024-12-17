using Microsoft.AspNetCore.Mvc;
using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Dto;

namespace SIMAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OnFieldController : BaseController
    {
        private readonly IOnFieldService _service;
        private readonly IConfiguration _configuration;
        public OnFieldController(IOnFieldService service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
        }


        [HttpPost("OnFieldActivationList")]
        public async Task<IActionResult> OnFieldActivationList(GetReportRequest request)
        {            
            GetReportFromAndToDates(request);
            var result = await _service.OnFieldActivationListAsync(request);

            return Json(result);
        }

        [HttpPost("OnFieldCommissionList")]
        public async Task<IActionResult> OnFieldCommissionList(GetReportRequest request)
        {
            
            GetReportFromAndToDates(request);
            var result = await _service.OnFieldCommissionListAsync(request);
            return Json(result);
        }

        [HttpPost("OnFieldGivenVSActivationList")]
        public async Task<IActionResult> OnFieldGivenVSActivationList(GetReportRequest request)
        {           
            GetReportFromAndToDates(request);
            var result = await _service.OnFieldGivenVSActivationListync(request);
            return Json(result);
        }

        [HttpGet("OnFieldShopVisitHistory")]
        public async Task<IActionResult> OnFieldShopVisitHistory(int shopId)
        {
            var result = await _service.OnFieldShopVisitHistoryAsync(shopId);
            return Json(result);
        }

        [HttpGet("OnFieildCommissionWalletAmounts")]
        public async Task<IActionResult> OnFieildCommissionWalletAmounts(int shopId)
        {
            var result = await _service.OnFieildCommissionWalletAmountsAsync(shopId);
            return Json(result);
        }

        [HttpGet("OnFieldCommissionWalletHistory")]
        public async Task<IActionResult> OnFieldCommissionWalletHistory(int shopId,string walletType)
        {
            var result = await _service.OnFieldCommissionWalletHistoryAsync(shopId, walletType);
            return Json(result);
        }



        #region private methods

        private void GetReportFromAndToDates(GetReportRequest request)
        {
            request.userId = GetUserId;
            request.userRole = GetUser.UserRole.RoleName;
            // Get the first day of the current month
            DateTime currentDate = DateTime.Now;
            DateTime firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            request.toDate = firstDayOfMonth.ToString("yyyy-MM-dd");
            request.fromDate = firstDayOfMonth.AddMonths(-6).ToString("yyyy-MM-dd");
        } 

        #endregion
    }
}