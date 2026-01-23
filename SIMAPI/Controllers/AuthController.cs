using Microsoft.AspNetCore.Mvc;
using SIMAPI.Business.Helper;
using SIMAPI.Business.Interfaces;
using SIMAPI.Business.Services;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Models.Login;

namespace SIMAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITrackService _trackService;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _config;

        public AuthController(IAuthService authService, ITrackService trackService, ITokenService tokenService, IConfiguration config)
        {
            _authService = authService;
            _trackService = trackService;
            _tokenService = tokenService;
            _config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _authService.GetUserDetailsAsync(dto.Username, dto.Password);
            if (user == null)
                return Unauthorized();
            var response = await _tokenService.GenerateTokens(user);

            UserTrackDto userTrack = new UserTrackDto();
            userTrack.UserId = user.userId;
            userTrack.TrackedDate = DateTime.Now;
            userTrack.CreatedDate = DateTime.Now;
            userTrack.WorkType = "Login";
            userTrack.Latitude = dto.Latitude;
            userTrack.Longitude = dto.Longitude;
            await _trackService.LogUserTrackAsync(userTrack);
            return Ok(response);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequestDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.RefreshToken))
                return BadRequest("Refresh token required");

            var refreshTokenHash = TokenHelpers.ComputeSha256Hash(dto.RefreshToken);
            var storedToken = await _tokenService.GetRefreshTokenByHashAsync(refreshTokenHash);

            if (storedToken == null)
                return Unauthorized("Invalid refresh token");

            if (storedToken.IsRevoked || storedToken.IsUsed)
                return Unauthorized("Refresh token already used");

            if (storedToken.ExpiresAt < DateTime.UtcNow)
                return Unauthorized("Refresh token expired");

            var user = storedToken.User;

            // ?? Generate new tokens FIRST
            var response = await _tokenService.GenerateTokens(user);

            // ?? Rotate refresh token
            storedToken.IsUsed = true;
            storedToken.IsRevoked = true;
            storedToken.ReplacedByTokenHash =
                TokenHelpers.ComputeSha256Hash(response.RefreshToken);

            await _tokenService.UpdateRefreshTokenAsync(storedToken);

            return Ok(response);
        }



        [HttpPost("retailerLogin")]
        public async Task<IActionResult> RetailerLogin([FromBody] LoginDto dto)
        {
            var user = await _authService.GetRetailerUserDetailsAsync(dto.Username, dto.Password);

            if (user == null)
            {
                AuthResponseDto invalid = new AuthResponseDto();
                invalid.StatusCode = 200;
                invalid.Message = "Invalid";
                return Ok(invalid);
            }
            var response = await _tokenService.GenerateTokens(user);
            response.Message = "Success";
            response.StatusCode = 200;
            return Ok(response);
        }


    }

}