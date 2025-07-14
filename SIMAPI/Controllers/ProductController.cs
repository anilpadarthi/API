using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using DocumentFormat.OpenXml.Office2010.Excel;
using SIMAPI.Business.Helper;

namespace SIMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseController
    {
        private readonly IProductService _service;
        private readonly IConfiguration _configuration;
        public ProductController(IProductService service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
        }


        [HttpPost("GetByPaging")]
        public async Task<IActionResult> GetByPaging(GetPagedSearch request)
        {
            var result = await _service.GetByPagingAsync(request);
            return Json(result);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Json(result);
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {           
            var result = await _service.GetByIdAsync(id);
            return Json(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromForm] ProductDto request)
        {
            var result = await _service.CreateAsync(request);
            return Json(result);
        }

        [HttpPost("AddProductImage")]
        public async Task<IActionResult> AddProductImage([FromForm] ProductImageModel request)
        {
            var result = await _service.AddProductImageAsync(request);
            return Json(result);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromForm] ProductDto request)
        {
            var result = await _service.UpdateAsync(request);
            return Json(result);
        }

        [HttpGet("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteProductAsync(id);
            return Json(result);
        }


        [HttpPost("CreateBundleProduct")]
        public async Task<IActionResult> CreateBundleProduct(BundleProductRequestModel request)
        {
            var result = await _service.CreateBundleProductAsync(request);
            return Json(result);
        }

        [HttpPost("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts(ProductSearchModel request)
        {
            var result = await _service.GetAllProductsAsync(request);
            return Json(result);
        }

        [HttpGet("ExportToExcel")]
        public async Task<IActionResult> ExportToExcel()
        {
            var result = await _service.GetAllAsync();
            string excelName = $"AreaList.xlsx";
            var stream = ExcelUtility.ConvertDataToExcelFormat<Product>(result.data as List<Product>);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }
    }
}
