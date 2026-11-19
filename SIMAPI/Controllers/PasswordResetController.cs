using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using SIMAPI.Business;
using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Models;
using SIMAPI.Data.Models.ResetPassword;
using System.Security.Cryptography;
using System.Text;

namespace SIMAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PasswordResetController : ControllerBase
    {
        private readonly PasswordResetService _passwordResetService;

        public PasswordResetController(PasswordResetService passwordResetService)
        {
            _passwordResetService = passwordResetService;
        }

        // POST: api/account/forgot-password
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(
            [FromBody] ForgotPasswordRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request?.Email))
            {
                return BadRequest(new
                {
                    message = "Please enter a valid email address."
                });
            }

           var result = await _passwordResetService.ForgotPasswordAsync(
                request);

            // Always return the same message.
            return Ok(new
            {
                message = result
            });
        }
        [HttpPost("verify")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyResetToken(
            [FromBody] VerifyResetTokenDto request)
        {
            if (request == null ||
                string.IsNullOrWhiteSpace(request.Token))
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid password reset link."
                });
            }

            var isValid =
                await _passwordResetService.VerifyResetTokenAsync(
                    request.Token);

            if (!isValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Password reset link is invalid or expired. " +
                              "Please request a new link."
                });
            }

            return Ok(new
            {
                success = true,
                message = "Reset link verified successfully."
            });
        }

        // POST: api/account/reset-password
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(
            [FromBody] ResetPasswordRequestDto request)
        {
            if (request == null ||
                string.IsNullOrWhiteSpace(request.Token) ||
                string.IsNullOrWhiteSpace(request.NewPassword) ||
                string.IsNullOrWhiteSpace(request.ConfirmPassword))
            {
                return BadRequest(new
                {
                    message = "All fields are required."
                });
            }

            if (request.NewPassword != request.ConfirmPassword)
            {
                return BadRequest(new
                {
                    message = "Passwords do not match."
                });
            }

            if (request.NewPassword.Length < 8 ||
                request.NewPassword.Length > 128)
            {
                return BadRequest(new
                {
                    message = "Password must contain between 8 and 128 characters."
                });
            }

            var result =
                await _passwordResetService.ResetPasswordAsync(request);

            if (!result)
            {
                return BadRequest(new
                {
                    message = "Password reset link is invalid or expired. " +
                              "Please request a new link."
                });
            }

            return Ok(new
            {
                message = "Your password has been reset successfully."
            });
        }


       
    }

}
