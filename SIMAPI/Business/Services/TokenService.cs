using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SIMAPI.Business.Helper;
using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models.Login;
using SIMAPI.Repository.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SIMAPI.Business.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;
        private readonly ITokenRepository _tokenRepository;

        public TokenService(IConfiguration config, ITokenRepository tokenRepository)
        {
            _config = config;
            _tokenRepository = tokenRepository;
        }

        public async Task<AuthResponseDto> GenerateTokens(LoggedInUserDto user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // create unique jti
            var jti = Guid.NewGuid().ToString();


            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.userName),
                new Claim("userId", user.userId.ToString()),
                 new Claim("userDetails", JsonConvert.SerializeObject(user)),
                new Claim(JwtRegisteredClaimNames.Jti, jti)
                // add roles/other claims as needed
            };

            var accessToken = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(_config["Jwt:AccessTokenMinutes"])),
                signingCredentials: creds
            );

            var refreshTokenPlain = Guid.NewGuid().ToString();
            var refreshTokenHash = TokenHelpers.ComputeSha256Hash(refreshTokenPlain);

            var refreshToken = new RefreshToken
            {
                TokenHash = refreshTokenHash,
                JwtId = jti,
                UserId = user.userId,
                ExpiresAt = DateTime.Now.AddHours(Convert.ToInt32(_config["Jwt:RefreshTokenHours"])),
                IsRevoked = false
            };

            await this.SaveRefreshTokenAsync(refreshToken);

            return new AuthResponseDto
            {
                AccessToken = new JwtSecurityTokenHandler().WriteToken(accessToken),
                RefreshToken = refreshToken.TokenHash,
                ExpiresAt = accessToken.ValidTo,
                UserDetails = user
            };
        }

        //public (string refreshTokenPlain, RefreshToken refreshTokenEntity) CreateRefreshToken(int userId, string jwtId, int hours)
        //{
        //    var plain = TokenHelpers.GenerateRandomToken(64);
        //    var hash = TokenHelpers.ComputeSha256Hash(plain);

        //    var entity = new RefreshToken
        //    {
        //        UserId = userId,
        //        TokenHash = hash,
        //        JwtId = jwtId,
        //        ExpiresAt = DateTime.Now.AddHours(hours),
        //        CreatedAt = DateTime.Now
        //    };

        //    return (plain, entity);
        //}

        public async Task SaveRefreshTokenAsync(RefreshToken token)
        {
            try
            {
                await _tokenRepository.SaveRefreshTokenAsync(token);
            }
            catch (Exception ex)
            {

            }
        }

        public async Task<RefreshToken?> GetRefreshTokenByHashAsync(string tokenHash)
        {
            return await _tokenRepository.GetRefreshTokenByHashAsync(tokenHash);
        }

        public async Task UpdateRefreshTokenAsync()
        {
            await _tokenRepository.SaveChangesAsync();
        }
    }

}
