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

        [HttpPost("DownloadTargetData")]
        public async Task<IActionResult> DownloadTargetData(GetReportRequest request)
        {
            var stream = await _service.DownloadTargetDataAsync(request);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Analysis.xlsx");
        }

        [HttpGet("DownloadTemplate")]
        public async Task<IActionResult> DownloadTemplate(string importFileType)
        {
            if (string.IsNullOrWhiteSpace(importFileType))
                return BadRequest(new { message = "importFileType is required" });

            try
            {
                // Templates folder can be configured via "TemplateFilesPath" in configuration.
                // Fallback: <appRoot>/wwwroot/templates
                var basePath = _configuration["AppSettings:UploadPath"];
                var templatesFolder = Path.Combine(basePath, "Resources", "Templates");

                // Map friendly import types to concrete template file names.
                var fileName = importFileType switch
                {
                    "OrderStatus" => "OrderStatus.xlsx",
                    "TrackNumber" => "TrackNumber.xlsx",
                    "Spam" => "Spam.xlsx",
                    "DailyActivation" => "DailyActivation.xlsx",
                    "Stock" => "Stock.xlsx",
                    "Target" => "Target.xlsx",
                    "AccessoriesStock" => "AccessoriesStock.xlsx",
                    "ShopCommissionCheque" => "ShopCommissionCheque.xlsx",
                    "BankChequeWithdraw" => "BankChequeWithdraw.xlsx",
                    _ => importFileType // allow passing a direct file name
                };

                // Prevent path traversal
                fileName = Path.GetFileName(fileName);
                var filePath = Path.Combine(templatesFolder, fileName);

                if (!System.IO.File.Exists(filePath))
                    return NotFound(new { message = $"Template '{fileName}' not found in '{templatesFolder}'." });

                var stream = System.IO.File.OpenRead(filePath);
                return await Task.FromResult(File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Failed to download template file.", detail = ex.Message });
            }
        }

    }
}