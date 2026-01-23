using SIMAPI.Data.Entities;
using SIMAPI.Data.Models.Login;

namespace SIMAPI.Business.Interfaces
{
    public interface ITokenService
    {
        Task<AuthResponseDto> GenerateTokens(LoggedInUserDto user);
        //(string refreshTokenPlain, RefreshToken refreshTokenEntity) CreateRefreshToken(int userId, string jwtId, int hours);
        Task SaveRefreshTokenAsync(RefreshToken token);
        Task<RefreshToken?> GetRefreshTokenByHashAsync(string tokenHash);
        Task UpdateRefreshTokenAsync(RefreshToken token);
    }
}
