using Microsoft.AspNetCore.Mvc;
using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Dto;

namespace SIMAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : BaseController
    {
        private readonly ICategoryService _service;
        private readonly IConfiguration _configuration;
        public CategoryController(ICategoryService service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
        }


        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromForm] CategoryDto request)
        {
            var result = await _service.CreateAsync(request);
            return Json(result);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromForm] CategoryDto request)
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


    }
}