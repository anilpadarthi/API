using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
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

        public string CreateAccessToken(LoggedInUserDto user)
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

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(_config["Jwt:AccessTokenMinutes"])),
                signingCredentials: creds
             );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }

        public RefreshToken CreateRefreshToken(int userId,string type)
        {
            var refreshToken = new RefreshToken
            {
                Token = Guid.NewGuid().ToString(),
                Created = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(
                    Convert.ToDouble(_config["Jwt:RefreshTokenMinutes"])),
                UserId = userId,
                IsActive = true,
                Type = type
            };

            _tokenRepository.Add(refreshToken);
            _tokenRepository.SaveChangesAsync();

            return refreshToken;
        }


        public async Task SaveRefreshTokenAsync(RefreshToken token)
        {
            // preserve previous behavior but don't swallow exceptions silently
            await _tokenRepository.SaveRefreshTokenAsync(token);
        }

        public async Task<RefreshToken?> GetRefreshTokenByHashAsync(string tokenHash)
        {
            return await _tokenRepository.GetRefreshTokenByHashAsync(tokenHash);
        }

        public async Task UpdateRefreshTokenAsync(RefreshToken token)
        {
            await _tokenRepository.UpdateAsync(token);
        }
    }

}
