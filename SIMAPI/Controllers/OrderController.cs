using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SIMAPI.Business.Helper;
using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Models.Export;

namespace SIMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : BaseController
    {
        private readonly IOrderService _service;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        public OrderController(IOrderService service, IConfiguration configuration, IMapper mapper)
        {
            _service = service;
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpGet("GetShoppingPageDetails")]
        public async Task<IActionResult> GetShoppingPageDetails()
        {
            var result = await _service.GetShoppingPageDetailsAsync();
            return Json(result);
        }

        [HttpGet("GetProductSearchList")]
        public async Task<IActionResult> GetProductSearchList(string searchText)
        {
            var result = await _service.GetProductSearchListAsync(searchText);
            return Json(result);
        }

        [HttpGet("GetProductList")]
        public async Task<IActionResult> GetProductList(int categoryId, int subCategoryId)
        {
            var result = await _service.GetProductListAsync(categoryId, subCategoryId);
            return Json(result);
        }

        [HttpGet("GetNewArrivals")]
        public async Task<IActionResult> GetNewArrivals()
        {
            var result = await _service.GetNewArrivalsAsync();
            return Json(result);
        }

        [HttpPost("GetPagedOrderList")]
        public async Task<IActionResult> GetPagedOrderList(GetPagedOrderListDto request)
        {
            request.loggedInUserId = GetUserId;
            request.loggedInUserRoleId = GetUser.userRoleId;

            var result = await _service.GetPagedOrderListAsync(request);
            return Json(result);
        }


        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            return Json(result);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create(OrderDetailDto request)
        {
            request.loggedInUserId = GetUserId;
            request.OrderedBy =  GetUser.userRole.RoleName;
            var result = await _service.CreateAsync(request);
            return Json(result);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update(OrderDetailDto request)
        {
            request.loggedInUserId = GetUserId;
            var result = await _service.UpdateAsync(request);
            return Json(result);
        }

        [HttpPost("UpdateStatus")]
        public async Task<IActionResult> UpdateStatus(OrderStatusModel request)
        {
            request.loggedInUserId = GetUserId;
            var result = await _service.UpdateStatusAsync(request);
            return Json(result);
        }

        [HttpPost("UpdateOrderDetails")]
        public async Task<IActionResult> UpdateOrderDetails(OrderStatusModel request)
        {
            request.loggedInUserId = GetUserId;
            var result = await _service.UpdateOrderDetailsAsync(request);
            return Json(result);
        }

        [HttpGet("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            //var result = await _service.DeleteAs(id);
            return Json("");
        }

        [HttpGet("GetOrderHistory/{orderId}")]
        public async Task<IActionResult> GetOrderHistory(int orderId)
        {
            var result = await _service.GetOrderHistoryAsync(orderId);
            return Json(result);
        }

        [HttpGet("GetOrderPaymentHistory/{orderId}")]
        public async Task<IActionResult> GetOrderPaymentHistory(int orderId)
        {
            var result = await _service.GetOrderPaymentHistoryAsync(orderId);
            return Json(result);
        }

        [HttpGet("UpdateOrderPayment/{orderPaymentId}")]
        public async Task<IActionResult> UpdateOrderPayment(int orderPaymentId)
        {
            var result = await _service.UpdateOrderPaymentAsync(orderPaymentId, GetUser.userRoleId);
            return Json(result);
        }

        [HttpGet("DeleteOrderPayment/{orderPaymentId}")]
        public async Task<IActionResult> DeleteOrderPayment(int orderPaymentId)
        {
            var result = await _service.DeleteOrderPaymentAsync(orderPaymentId);
            return Json(result);
        }

        [HttpPost("DownloadOrders")]
        public async Task<IActionResult> DownloadOrders(GetPagedOrderListDto request)
        {
            var result = await _service.DownloadOrderListAsync(request);
            var lst = _mapper.Map<List<ExportSaleOrder>>(result.data);
            var stream = ExcelUtility.ConvertDataToExcelFormat<ExportSaleOrder>(lst);
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Sales.xlsx");
        }

        [HttpPost("DownloadOrderListJson")]
        public async Task<IActionResult> DownloadOrderListJson(GetPagedOrderListDto request)
        {
            //GetPagedOrderListRequest request = new GetPagedOrderListRequest();
            request.requestType = "Download";
            var result = await _service.GetPagedOrderListAsync(request);
            return Json(result);
        }

        [HttpGet("GeneratePdfInvoice")]
        public async Task<IActionResult> GeneratePdfInvoice(int orderId, bool isVAT)
        {
            var result = await _service.GeneratePdfInvoiceAsync(orderId, isVAT);
            byte[] byteInfo = result.data as byte[];
            return File(byteInfo, "application/pdf", "Invoice_" + orderId + ".pdf");
        }

        [HttpGet("SendInvoice")]
        public async Task<IActionResult> SendInvoice(int orderId, bool isVAT)
        {
            var result = await _service.SendInvoiceAsync(orderId, isVAT);
            return Json(result);
        }


        [HttpGet("GetOrderNotificationCount")]
        public async Task<IActionResult> GetOrderNotificationCount()
        {
            var result = await _service.GetOrderNotificationCountAsync();
            return Json(result);
        }

        [HttpPost("CreatePayment")]
        public async Task<IActionResult> CreatePayment([FromForm] OrderPaymentDto request)
        {
            request.UserId = GetUserId;
            var result = await _service.CreateOrderPaymentAsync(request);
            return Json(result);
        }



        [HttpGet("LoadOutstandingMetrics")]
        public async Task<IActionResult> LoadOutstandingMetrics(string filterType, int filterId)
        {
            var result = await _service.LoadOutstandingMetricsAsync(filterType, filterId);
            return Json(result);
        }

        [HttpGet("HideOrder")]
        public async Task<IActionResult> HideOrder(int orderId, bool isHide)
        {
            var result = await _service.HideOrderAsync(orderId, isHide);
            return Json(result);
        }
    }
}
