using SIMAPI.Data.Entities;

namespace SIMAPI.Repository.Interfaces
{
    public interface ITokenRepository: IRepository
    {
        Task SaveRefreshTokenAsync(RefreshToken token);
        Task<RefreshToken?> GetRefreshTokenByHashAsync(string tokenHash);
        Task UpdateAsync(RefreshToken token);
    }
}
