using Microsoft.AspNetCore.Mvc;
using SIMAPI.Business.Helper;
using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models.CommissionStatement;

namespace SIMAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommissionStatementController : BaseController
    {
        private readonly ICommissionStatementService _service;
        private readonly IConfiguration _configuration;
        public CommissionStatementController(ICommissionStatementService service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
        }


        [HttpPost("GetCommissionStatementReport")]
        public async Task<IActionResult> GetCommissionStatementReport(GetReportRequest request)
        {
            var result = await _service.DownloadPDFStatementReportAsync(request);
            return File(result, "application/pdf", "CommissionStatement" + request.shopId + ".pdf");
        }

        [HttpPost("GetCommissionStatementVATReportAsync")]
        public async Task<IActionResult> GetCommissionStatementVATReport(GetReportRequest request)
        {
            var result = await _service.DownloadVATStatementReportAsync(request);
            return File(result, "application/pdf", "CommissionStatement" + request.shopId + ".pdf");
        }

        [HttpPost("GetCommissionList")]
        public async Task<IActionResult> GetCommissionListAsync(GetReportRequest request)
        {
            var result = await _service.GetCommissionListAsync(request);
            return Json(result);
        }

        [HttpGet("GetCommissionHistoryDetails")]
        public async Task<IActionResult> GetCommissionHistoryDetails(int shopCommissionHistoryId)
        {
            var result = await _service.GetCommissionHistoryDetailsAsync(shopCommissionHistoryId);
            return Json(result);
        }

        [HttpGet("OptInForShopCommission")]
        public async Task<IActionResult> OptInForShopCommission(int shopCommissionHistoryId, string optInType)
        {
            var result = await _service.OptInForShopCommissionAsync(shopCommissionHistoryId, optInType, GetUserId);
            return Json(result);
        }



        [HttpGet("DownloadCommissionStatement")]
        public async Task<IActionResult> DownloadCommissionStatement(int shopId, string fromDate)
        {
            GetReportRequest request = new GetReportRequest();
            request.shopId = shopId;
            request.fromDate = fromDate;
            var result = await _service.DownloadPDFStatementReportAsync(request);
            byte[] byteInfo = result as byte[];
            return File(byteInfo, "application/pdf", "Commission_Statement_" + shopId + ".pdf");
        }

        [HttpGet("ExportCommissionChequeExcel")]
        public async Task<IActionResult> ExportCommissionChequeExcel(bool isOptedIn, string fromDate)
        {
            GetReportRequest request = new GetReportRequest();
            request.isOptedForCheques = isOptedIn;
            request.fromDate = fromDate;
            var result = await _service.ExportCommissionChequeExcelAsync(request);
            string excelName = $"CommissionList_" + fromDate + ".xlsx";
            var stream = ExcelUtility.ConvertDataToExcelFormat<ExportCommissionList>(result.ToList());
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        [HttpGet("HideBonus")]
        public async Task<IActionResult> HideBonus(int shopCommissionHistoryId, bool isDisplayBonus)
        {
            var result = await _service.HideBonusAsync(shopCommissionHistoryId, isDisplayBonus);
            return Json(result);
        }

    }
}