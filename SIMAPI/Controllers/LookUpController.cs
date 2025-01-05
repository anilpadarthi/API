using Microsoft.AspNetCore.Mvc;
using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Dto;

namespace SIMAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LookUpController : BaseController
    {
        private readonly ILookUpService _service;
        private readonly IConfiguration _configuration;
        public LookUpController(ILookUpService service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
        }


        [HttpGet("Areas")]
        public async Task<IActionResult> GetAreaLookup()
        {
            GetLookupRequest request = new GetLookupRequest();
            request.userId = GetUserId;
            request.userRoleId = GetUser.UserRoleId;
            var result = await _service.GetAreaLookupAsync(request);
            return Json(result);
        }

        [HttpGet("Shops")]
        public async Task<IActionResult> GetShopLookup(int areaId)
        {
            var result = await _service.GetShopLookupAsync(areaId);
            return Json(result);
        }

        [HttpGet("Networks")]
        public async Task<IActionResult> GetNetworkLookup()
        {
            var result = await _service.GetNetworkLookupAsync();
            return Json(result);
        }

        [HttpGet("Agents")]
        public async Task<IActionResult> GetAgentLookup()
        {
            GetLookupRequest request = new GetLookupRequest();
            request.userId = GetUserId;
            request.userRoleId = GetUser.UserRoleId;
            request.filterType = "Agents";
            var result = await _service.GetUserLookupAsync(request);
            return Json(result);
        }

        [HttpGet("Managers")]
        public async Task<IActionResult> GetManagerLookup()
        {
            GetLookupRequest request = new GetLookupRequest();
            request.userId = GetUserId;
            request.userRoleId = GetUser.UserRoleId;
            request.filterType = "Managers";
            var result = await _service.GetUserLookupAsync(request);
            return Json(result);
        }

        [HttpGet("Roles")]
        public async Task<IActionResult> GetUserRoleLookup()
        {
            var result = await _service.GetUserRoleLookupAsync();
            return Json(result);
        }

        [HttpGet("Suppliers")]
        public async Task<IActionResult> GetSupplierLookup()
        {
            var result = await _service.GetSupplierLookupAsync();
            return Json(result);
        }

        [HttpGet("SupplierAccounts")]
        public async Task<IActionResult> GetSupplierAccountLookup(int supplierId)
        {
            var result = await _service.GetSupplierAccountLookupAsync(supplierId);
            return Json(result);
        }

        [HttpGet("Categories")]
        public async Task<IActionResult> GetCategories()
        {
            var result = await _service.GetCategories();
            return Json(result);
        }

        [HttpGet("{categoryId}/SubCategories")]
        public async Task<IActionResult> GetSubCategories(int categoryId)
        {
            var result = await _service.GetSubCategories(categoryId);
            return Json(result);
        }

        [HttpGet("Colours")]
        public async Task<IActionResult> GetAvailableColours()
        {
            var result = await _service.GetAvailableColours();
            return Json(result);
        }

        [HttpGet("Sizes")]
        public async Task<IActionResult> GetAvailableSizes()
        {
            var result = await _service.GetAvailableSizes();
            return Json(result);
        }

        [HttpGet("ConfigurationTypes")]
        public async Task<IActionResult> GetConfigurationTypes()
        {
            var result = await _service.GetConfigurationTypes();
            return Json(result);
        }

        [HttpGet("Products")]
        public async Task<IActionResult> GetProducts()
        {
            var result = await _service.GetProducts();
            return Json(result);
        }


    }
}