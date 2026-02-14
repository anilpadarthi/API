using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SIMAPI.Business.Helper;
using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Models;
using SIMAPI.Data.Models.Export;
using SIMAPI.Data.Models.OnField;

namespace SIMAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShopController : BaseController
    {
        private readonly IShopService _service;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public ShopController(IShopService service, IConfiguration configuration, IMapper mapper)
        {
            _service = service;
            _configuration = configuration;
            _mapper = mapper;

        }


        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromForm] ShopDto request)
        {
            request.CreatedBy = GetUserId;
            var result = await _service.CreateAsync(request);
            return Json(result);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromForm] ShopDto request)
        {
            request.CreatedBy = GetUserId;
            request.ModifiedBy = GetUserId;
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

        [HttpPost("GetByPaging")]
        public async Task<IActionResult> GetByPaging(GetPagedSearch request)
        {
            var result = await _service.GetByPagingAsync(request);
            return Json(result);
        }

        [HttpPost("ShopVisit")]
        public async Task<IActionResult> ShopVisit([FromForm] ShopVisitRequestmodel request)
        {
            request.UserId = GetUserId;
            var result = await _service.ShopVisitAsync(request);
            return Json(result);
        }

        [HttpGet("GetShopVisitHistory")]
        public async Task<IActionResult> GetShopVisitHistory(int shopId)
        {
            var result = await _service.GetShopVisitHistoryAsync(shopId);
            return Json(result);
        }

        [HttpGet("GetShopAgreementHistory")]
        public async Task<IActionResult> GetShopAgreementHistory(int shopId)
        {
            var result = await _service.GetShopAgreementHistoryAsync(shopId);
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

        [HttpGet("GetShopAddressDetails")]
        public async Task<IActionResult> GetShopAddressDetails(int shopId)
        {
            var result = await _service.GetShopAddressDetailsAsync(shopId);
            return Json(result);
        }

        [HttpPost("UpdateAddress")]
        public async Task<IActionResult> UpdateAddress(ShippingAddressDetails request)
        {
            var userId = GetUserId;
            var result = await _service.UpdateAddressAsync(request);
            return Json(result);
        }

        [HttpGet("SendActivationEmail")]
        public async Task<IActionResult> SendActivationEmail(int shopId)
        {
            var result = await _service.SendActivationEmailAsync(shopId);
            return Json(result);
        }

        [HttpGet("ExportToExcel")]
        public async Task<IActionResult> ExportToExcel(int areaId = 0)
        {
            try
            {
                var result = await _service.GetAllAsync(areaId);
                string excelName = $"ShopList.xlsx";
                var lst = _mapper.Map<List<ExportShop>>(result.data);
                var stream = ExcelUtility.ConvertDataToExcelFormat<ExportShop>(lst);
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
            catch(Exception ex)
            {
                return File("", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
            }
        }

        [HttpGet("GetShopCommissionCheques")]
        public async Task<IActionResult> GetShopCommissionCheques(int shopId)
        {
            var result = await _service.GetShopCommissionChequesAsync(shopId, "ShopCheques");
            return Json(result);
        }

        [HttpGet("GetShopCommissionCheque")]
        public async Task<IActionResult> GetShopCommissionCheque(int sno)
        {
            var result = await _service.GetShopCommissionChequeAsync(sno);
            return Json(result);
        }

        [HttpGet("UpdateShopCommissionCheque")]
        public async Task<IActionResult> UpdateShopCommissionCheque(int sno, string chequeNumber)
        {
            var result = await _service.UpdateShopCommissionChequeAsync(sno, chequeNumber);
            return Json(result);
        }

        [HttpGet("DeleteShopCommissionCheque")]
        public async Task<IActionResult> DeleteShopCommissionCheque(int sno)
        {
            var result = await _service.DeleteShopCommissionChequeAsync(sno);
            return Json(result);
        }

        [HttpGet("GlobalShopSearch")]
        public async Task<IActionResult> GlobalShopSearch(string searchText)
        {
            GetLookupRequest request = new GetLookupRequest();
            request.userId = GetUserId;
            request.userRoleId = GetUser.userRoleId;
            request.searchText = searchText;
            var result = await _service.GlobalShopSearchAsync(request);
            return Json(result);
        }

    }
}