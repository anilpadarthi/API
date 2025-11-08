using Microsoft.AspNetCore.Mvc;
using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;

namespace SIMAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ManagementController : BaseController
    {
        private readonly IManagementService _service;
        private readonly IConfiguration _configuration;
        public ManagementController(IManagementService service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
        }


        [HttpPost("CreateWhatsAppNotificationRequest")]
        public async Task<IActionResult> CreateWhatsAppNotificationRequest(WhatsAppRequestDto request)
        {
            request.UserId = GetUserId;
            request.UserType = GetUser.userRole.RoleName;
            var result = await _service.CreateWhatsAppNotificationRequestAsync(request);
            return Json(result);
        }

        [HttpGet("GetUserSalaryTransaction")]
        public async Task<IActionResult> GetUserSalaryTransaction(int userSalaryTransactionID)
        {
            var result = await _service.GetUserSalaryTransactionAsync(userSalaryTransactionID);
            return Json(result);
        }

       

        [HttpPost("CreateUserSalaryTransaction")]
        public async Task<IActionResult> CreateUserSalaryTransaction(UserSalaryTransaction request)
        {
            var result = await _service.CreateUserSalaryTransactionAsync(request);
            return Json(result);
        }


        [HttpPut("UpdateUserSalaryTransaction")]
        public async Task<IActionResult> UpdateUserSalaryTransaction(UserSalaryTransaction request)
        {
            var result = await _service.UpdateUserSalaryTransactionAsync(request);
            return Json(result);
        }

        [HttpDelete("DeleteUserSalaryTransaction")]
        public async Task<IActionResult> DeleteUserSalaryTransaction(int userSalaryTransactionID)
        {
            var result = await _service.DeleteUserSalaryTransactionAsync(userSalaryTransactionID);
            return Json(result);
        }



    }
}