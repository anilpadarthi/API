using Microsoft.AspNetCore.Mvc;
using SIMAPI.Business.Helper;
using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Models.Login;

namespace SIMAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;
        private readonly IConfiguration _config;

        public AuthController(IAuthService authService, ITokenService tokenService, IConfiguration config)
        {
            _authService = authService; _tokenService = tokenService; _config = config;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _authService.GetUserDetailsAsync(dto.Username, dto.Password);
            if (user == null) 
                return Unauthorized();
            var response = await _tokenService.GenerateTokens(user);
            return Ok(response);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequestDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.RefreshToken)) 
                return BadRequest("Refresh token required");

            var hash = TokenHelpers.ComputeSha256Hash(dto.RefreshToken);
            var storedToken = await _tokenService.GetRefreshTokenByHashAsync(dto.RefreshToken);

            if (storedToken == null || storedToken.IsRevoked || storedToken.ExpiresAt < DateTime.Now)
                return Unauthorized("Invalid or expired refresh token");

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

        //[HttpPost("retailerLogin")]
        //public async Task<IActionResult> RetailerLogin([FromBody] LoginDto dto)
        //{
        //    var user = await _authService.GetRetailerUserDetailsAsync(dto.Username, dto.Password);
        //    if (user == null) return Unauthorized();

        //    var accessToken = _tokenService.CreateAccessToken(user, int.Parse(_config["Jwt:AccessTokenMinutes"]));
        //    // retrieve JWT id from token to tie refresh token to it
        //    var handler = new JwtSecurityTokenHandler();
        //    var jwt = handler.ReadJwtToken(accessToken);
        //    var jti = jwt.Id; // jti claim

        //    var (refreshPlain, refreshEntity) = _tokenService.CreateRefreshToken(user.userId, jti, int.Parse(_config["Jwt:RefreshTokenHours"]));
        //    await _tokenService.SaveRefreshTokenAsync(refreshEntity);

        //    var response = new AuthResponseDto { AccessToken = accessToken, RefreshToken = refreshPlain, UserDetails = user };
        //    return Ok(response);
        //}


    }

}