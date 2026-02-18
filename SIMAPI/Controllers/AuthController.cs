using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
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
            try
            {                
                var user = await _authService.GetUserDetailsAsync(dto.Username, dto.Password);
                if (user == null)
                    return Unauthorized("Invalid user credentials");
                var accessToken = _tokenService.CreateAccessToken(user);
                var refreshToken = await _tokenService.CreateRefreshToken(user.userId, "user");

                UserTrackDto userTrack = new UserTrackDto();
                userTrack.UserId = user.userId;
                userTrack.TrackedDate = DateTime.Now;
                userTrack.CreatedDate = DateTime.Now;
                userTrack.WorkType = "Login";
                userTrack.Latitude = dto.Latitude;
                userTrack.Longitude = dto.Longitude;
                await _trackService.LogUserTrackAsync(userTrack);

                return Ok(new
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken.Token
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred during login.");
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(TokenRequest request)
        {
            try
            {
                var savedRefreshToken = await _tokenService.GetRefreshTokenByHashAsync(request.RefreshToken);

                if (savedRefreshToken == null || !savedRefreshToken.IsActive || savedRefreshToken.Expires <= DateTime.UtcNow)
                {
                    savedRefreshToken.IsActive = false;
                    savedRefreshToken.IsExpired = true;
                    await _tokenService.UpdateRefreshTokenAsync(savedRefreshToken);
                    return Unauthorized("Invalid refresh token");
                }

                var user = await _authService.GetUserDetailsByUserIdAsync(savedRefreshToken.UserId);

                if (user == null)
                    return Unauthorized("Invalid user");

                var newAccessToken = _tokenService.CreateAccessToken(user);

                return Ok(new
                {
                    AccessToken = newAccessToken
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred during token refresh.");
            }
        }




        [HttpPost("retailerLogin")]
        public async Task<IActionResult> RetailerLogin([FromBody] LoginDto dto)
        {
            try
            {
                var user = await _authService.GetRetailerUserDetailsAsync(dto.Username, dto.Password);

                if (user == null)
                    return Unauthorized("Invalid credentials");
                var accessToken = _tokenService.CreateAccessToken(user);
                var refreshToken = await _tokenService.CreateRefreshToken(user.userId, "Retailer");
                return Ok(new
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken.Token
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred during login.");
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout(TokenRequest request)
        {
            var token = await _tokenService.GetRefreshTokenByHashAsync(request.RefreshToken);

            if (token == null)
                return NotFound();

            token.Revoked = DateTime.UtcNow;
            token.IsExpired = true;
            token.IsActive = false;
            await _tokenService.UpdateRefreshTokenAsync(token);

            return Ok("Logged out successfully");
        }
    }

}