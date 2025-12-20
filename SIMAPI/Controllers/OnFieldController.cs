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
            // 14  represents to pull last one year commissions we are ahead of 2 months
            GetReportFromAndToDates(request,14);
            var result = await _service.OnFieldCommissionListAsync(request);
            return Json(result);
        }

        [HttpPost("OnFieldGivenVSActivationList")]
        public async Task<IActionResult> OnFieldGivenVSActivationList(GetReportRequest request)
        {           
            GetReportFromAndToDates(request);
            DateTime currentDate = DateTime.Now;
            DateTime firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            request.fromDate = firstDayOfMonth.AddMonths(-3).ToString("yyyy-MM-dd");
            var result = await _service.OnFieldGivenVSActivationListync(request);
            return Json(result);
        }

        [HttpPost("OnFieldSimConversionList")]
        public async Task<IActionResult> OnFieldSimConversionList(GetReportRequest request)
        {
            GetReportFromAndToDates(request);
            DateTime currentDate = DateTime.Now;
            DateTime firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            request.fromDate = firstDayOfMonth.AddMonths(-12).ToString("yyyy-MM-dd");
            var result = await _service.OnFieldSimConversionListAsync(request);
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

        [HttpGet("OutstandingBalance")]
        public async Task<IActionResult> OutstandingBalance(int shopId)
        {
            var result = await _service.OutstandingBalanceAsync(shopId);
            return Json(result);
        }



        #region private methods

        private void GetReportFromAndToDates(GetReportRequest request, int months = 6)
        {
            request.userId = GetUserId;
            request.userRole = GetUser.userRole.RoleName;
            // Get the first day of the current month
            DateTime currentDate = DateTime.Now;
            DateTime firstDayOfMonth = new DateTime(currentDate.Year, currentDate.Month, 1);
            request.toDate = firstDayOfMonth.ToString("yyyy-MM-dd");
            request.fromDate = firstDayOfMonth.AddMonths(-months).ToString("yyyy-MM-dd");
        } 

        #endregion
    }
}