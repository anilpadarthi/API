using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Models;

namespace SIMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordResetController : ControllerBase
    {
        private readonly IUserService _userService;

        public PasswordResetController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("request-reset")]
        public async Task<IActionResult> RequestPasswordReset(string email)
        {
            var token = await _userService.GenerateResetTokenAsync(email);
            if (token == null)
            {
                return NotFound("User not found");
            }

            return Ok("Password reset link sent to your email.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            var isValid = await _userService.ValidateTokenAsync(model.Token);
            if (!isValid)
            {
                return BadRequest("Invalid or expired token.");
            }

            var isPasswordUpdated = await _userService.ResetPasswordAsync(model.Token, model.NewPassword, model.ConfirmPassword);
            if (!isPasswordUpdated)
            {
                return BadRequest("Password update failed.");
            }

            return Ok("Password successfully updated.");
        }
    }
}
