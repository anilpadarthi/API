using Microsoft.AspNetCore.Mvc;
using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Models.OnField;

namespace SIMAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShopController : BaseController
    {
        private readonly IShopService _service;
        private readonly IConfiguration _configuration;
        public ShopController(IShopService service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
        }


        [HttpPost("Create")]
        public async Task<IActionResult> Create(ShopDto request)
        {
            request.CreatedBy = GetUserId;
            var result = await _service.CreateAsync(request);
            return Json(result);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update(ShopDto request)
        {
            request.CreatedBy = GetUserId;
            request.ModifiedBy = GetUserId;
            var result = await _service.UpdateAsync(request);
            return Json(result);
        }

        [HttpDelete("Delete")]
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

        [HttpPost("ShopVisit")]
        public async Task<IActionResult> ShopVisit(ShopVisitRequestmodel request)
        {
            var result = await _service.ShopVisitAsync(request);
            return Json(result);
        }

        [HttpGet("GetShopVisitHistory")]
        public async Task<IActionResult> GetShopVisitHistory(int shopId)
        {
            var result = await _service.GetShopVisitHistoryAsync(shopId);
            return Json(result);
        }

        [HttpGet("GetShopWalletAmount")]
        public async Task<IActionResult> GetShopWalletAmount(int shopId)
        {
            var result = await _service.GetShopWalletAmountAsync(shopId);
            return Json(result);
        }

        [HttpGet("GetShopWalletHistory")]
        public async Task<IActionResult> GetShopWalletHistory(int shopId, string walletType)
        {
            var result = await _service.GetShopWalletHistoryAsync(shopId, walletType);
            return Json(result);
        }

    }
}