using Microsoft.AspNetCore.Mvc;
using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Dto;

namespace SIMAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : BaseController
    {
        private readonly IUserService _service;
        private readonly IConfiguration _configuration;
        public UserController(IUserService service, IConfiguration configuration)
        {
            _service = service;
            _configuration = configuration;
        }


        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromForm] UserDto request)
        {
            request.CreatedBy = GetUserId;
            request.UpdatedBy = GetUserId;
            var result = await _service.CreateUserAsync(request);
            return Json(result);
        }

        [HttpPost("Update")]
        public async Task<IActionResult> Update([FromForm] UserDto request)
        {
            request.CreatedBy = GetUserId;
            request.UpdatedBy = GetUserId;
            var result = await _service.UpdateUserAsync(request);
            return Json(result);
        }

        [HttpGet("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteUserAsync(id);
            return Json(result);
        }


        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetUserByIdAsync(id);
            return Json(result);
        }

        [HttpGet("GetByName")]
        public async Task<IActionResult> GetByName(string name)
        {
            var result = await _service.GetUserByNameAsync(name);
            return Json(result);
        }

        [HttpPost("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllUsersAsync();
            return Json(result);
        }

        [HttpPost("GetByPaging")]
        public async Task<IActionResult> GetByPaging(GetPagedSearch request)
        {
            var result = await _service.GetUsersByPagingAsync(request);
            return Json(result);
        }

        [HttpPost("Download")]
        public async Task<IActionResult> Download(GetPagedSearch request)
        {
            var result = await _service.GetUsersByPagingAsync(request);
            return Json(result);
        }

        [HttpPost("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword(UserDto request)
        {
            var result = await _service.UpdateUserPasswordAsync(request);
            return Json(result);
        }

        [HttpPost("GetAllAgentsToAllocate")]
        public async Task<IActionResult> GetAllAgentsToAllocate()
        {
            var result = await _service.GetAllAgentsToAllocateAsync();
            return Json(result);
        }

        [HttpPost("AllocateAgentsToManager")]
        public async Task<IActionResult> AllocateAgentsToManager(AllocateAgentDto request)
        {
            var result = await _service.AllocateAgentsToUserAsync(request);
            return Json(result);
        }

        [HttpGet("ViewUserAllocationHistory")]
        public async Task<IActionResult> ViewUserAllocationHistory(int userId)
        {
            var result = await _service.ViewUserAllocationHistorySync(userId);
            return Json(result);
        }

        [HttpPost("UpdateAddress")]
        public async Task<IActionResult> UpdateAddress(string shippingAddress)
        {
            var userId = GetUserId;
            var result = await _service.UpdateAddressAsync(userId,shippingAddress);
            return Json(result);
        }

        [HttpGet("SendActivationEmail")]
        public async Task<IActionResult> SendActivationEmail(int userId)
        {
            var result = await _service.SendActivationEmailAsync(userId);
            return Json(result);
        }


    }
}