using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.AspNetCore.Mvc;
using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Dto;

namespace SIMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : BaseController
    {
        private readonly IOrderService _service;
        private readonly IConfiguration _configuration;
        public OrderController(IOrderService service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
        }

        [HttpGet("GetShoppingPageDetails")]
        public async Task<IActionResult> GetShoppingPageDetails()
        {
            var result = await _service.GetShoppingPageDetailsAsync();
            return Json(result);
        }

        [HttpPost("GetPagedOrderList")]
        public async Task<IActionResult> GetPagedOrderList(GetPagedOrderListDto request)
        {
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

        [HttpPost("DownloadOrders")]
        public async Task<IActionResult> DownloadOrders(GetPagedOrderListDto request)
        {
            //GetPagedOrderListRequest request = new GetPagedOrderListRequest();
            request.requestType = "Download";
            var result = await _service.DownloadOrderListAsync(request);
            //return Json(result);
            var byteData = (byte[])result.data;
            return File(byteData, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "order_list.xlsx");
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
        public async Task<IActionResult> GeneratePdfInvoice(int orderId)
        {
            MemoryStream workStream = new MemoryStream();
            Document document = new Document(PageSize.A4, 5f, 5f, 15f, 15f);
            Font NormalFont = FontFactory.GetFont("Arial", 10, Font.NORMAL, BaseColor.BLACK);
            PdfWriter writer = PdfWriter.GetInstance(document, workStream);

            document.Open();
            PdfPTable table = new PdfPTable(1);
            table.TotalWidth = 550f;
            table.LockedWidth = true;

            var cell = PhraseCell(new Phrase("Welcome to Leap", FontFactory.GetFont("Arial", 16, Font.NORMAL, BaseColor.BLACK)), PdfPCell.ALIGN_CENTER);
            table.AddCell(cell);

            document.Add(table);
            document.Close();
            byte[] byteInfo = workStream.ToArray();
            return File(byteInfo, "application/pdf", "sample.pdf");
        }


        [HttpGet("GetOrderNotificationCount")]
        public async Task<IActionResult> GetOrderNotificationCount()
        {
            var result = await _service.GetOrderNotificationCountAsync();
            return Json(result);
        }
        private static PdfPCell PhraseCell(Phrase phrase, int align)
        {
            PdfPCell cell = new PdfPCell(phrase);
            cell.BorderColor = BaseColor.WHITE;
            cell.VerticalAlignment = PdfPCell.ALIGN_TOP;
            cell.HorizontalAlignment = align;
            cell.PaddingBottom = 2f;
            cell.PaddingTop = 0f;
            return cell;
        }


    }
}
