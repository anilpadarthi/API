using Microsoft.AspNetCore.Mvc;
using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Dto;

namespace SIMAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseController : BaseController
    {
        private readonly IPurchaseService _service;

        public PurchaseController(IPurchaseService service)
        {
            _service = service;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePurchase(PurchaseInvoiceCreateDto request)
        {
            request.CreatedDate = DateTime.Now;
            request.CreatedBy = GetUserId;
            var result = await _service.CreatePurchaseAsync(request);
            return Json(result);
        }


        [HttpPost("Update")]
        public async Task<IActionResult> Update(PurchaseInvoiceCreateDto request)
        {
            request.CreatedBy = GetUserId;
            var result = await _service.UpdatePurchaseAsync(request);
            return Json(result);
        }



        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return Json(result);
        }

        [HttpGet("GetItems")]
        public async Task<IActionResult> GetItems(int id)
        {
            var result = await _service.GetItemsAsync(id);
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