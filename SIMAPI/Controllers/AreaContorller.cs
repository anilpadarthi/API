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
    public class AreaController : BaseController
    {
        private readonly IAreaService _service;
        private readonly IConfiguration _configuration;
        public AreaController(IAreaService service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
        }


        [HttpPost("Create")]
        public async Task<IActionResult> Create(AreaDto request)
        {
            var result = await _service.CreateAsync(request);
            return Json(result);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update(AreaDto request)
        {
            var result = await _service.UpdateAsync(request);
            return Json(result);
        }

        [HttpGet("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            return Json(result);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return Json(result);
        }

        [HttpGet("GetByName")]
        public async Task<IActionResult> GetByName(string name)
        {
            var result = await _service.GetByNameAsync(name);
            return Json(result);
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Json(result);
        }

        [HttpPost("GetByPaging")]
        public async Task<IActionResult> GetByPaging(GetPagedSearch request)
        {
            var result = await _service.GetByPagingAsync(request);
            return Json(result);
        }

        [HttpPost("GetAllAreasToAllocate")]
        public async Task<IActionResult> GetAllAreasToAllocate()
        {
            var result = await _service.GetAllAreasToAllocateAsync();
            return Json(result);
        }

        [HttpPost("AllocateAreasToAgent")]
        public async Task<IActionResult> AllocateAreasToAgent(AllocateAreaDto request)
        {
            var result = await _service.AllocateAreasToUserAsync(request);
            return Json(result);
        }

        [HttpGet("ExportToExcel")]
        public async Task<IActionResult> ExportToExcel()
        {
            var result = await _service.GetAllAsync();
            string excelName = $"AreaList.xlsx";
            var stream = ExcelUtility.ConvertDataToExcelFormat<Area>(result.ToList());
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            
        }

        [HttpGet("ViewAreaAllocationHistory")]
        public async Task<IActionResult> ViewAreaAllocationHistory(int areaId)
        {
            var result = await _service.ViewAreaAllocationHistorySync(areaId);
            return Json(result);
        }

    }
}