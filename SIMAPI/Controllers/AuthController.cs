using Microsoft.AspNetCore.Mvc;
using SIMAPI.Business.Helper;
using SIMAPI.Business.Interfaces;
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

            var hash = TokenHelpers.ComputeSha256Hash(dto.RefreshToken);
            var storedToken = await _tokenService.GetRefreshTokenByHashAsync(hash);


            if (storedToken == null || storedToken.IsRevoked || storedToken.ExpiresAt < DateTime.Now)
                return Unauthorized("Invalid or expired refresh token");
            storedToken.IsRevoked = true;
            await _tokenService.UpdateRefreshTokenAsync();
            // create new access token
            var user = storedToken.User;
            var response = await _tokenService.GenerateTokens(user);

            return Ok(response);
        }

        //[Authorize]
        //[HttpPost("revoke")]
        //public async Task<IActionResult> Revoke([FromBody] RefreshRequestDto dto)
        //{
        //    if (string.IsNullOrWhiteSpace(dto.RefreshToken)) return BadRequest();

        //    var hash = TokenHelpers.ComputeSha256Hash(dto.RefreshToken);
        //    var stored = await _tokenService.GetRefreshTokenByHashAsync(hash);
        //    if (stored == null) return NotFound();

        //    stored.IsRevoked = true;
        //    await _tokenService.UpdateRefreshTokenAsync();
        //    return Ok();
        //}

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