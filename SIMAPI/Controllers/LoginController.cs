using Microsoft.AspNetCore.Mvc;
using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Models;

namespace SIMAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly IAuthService _service;
        public LoginController(IAuthService service)
        {
            _service = service;
        }


        [HttpPost("ValidateUser")]
        public async Task<IActionResult> ValidateUser(string email, string password)
        {
            var result = await _service.ValidateUser(email, password);
            return Json(result);
        }

        [HttpPost("Authenticate")]
        public async Task<IActionResult> ValidateUser(LoginRequest request)
        {
            var result = await _service.ValidateUser(request.Email, request.Password);
            return Json(result);
        }


    }
}