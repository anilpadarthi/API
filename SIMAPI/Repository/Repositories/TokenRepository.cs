using Microsoft.EntityFrameworkCore;
using SIMAPI.Data;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models.Login;
using SIMAPI.Repository.Interfaces;

namespace SIMAPI.Repository.Repositories
{
    public class TokenRepository : Repository, ITokenRepository
    {

        public TokenRepository(SIMDBContext context) : base(context)
        {
        }

        public async Task SaveRefreshTokenAsync(RefreshToken token)
        {
            _context.Add(token);
            await _context.SaveChangesAsync();
        }

        public async Task<RefreshToken?> GetRefreshTokenByHashAsync(string tokenHash)
        {
            return await _context.Set<RefreshToken>()
               .FirstOrDefaultAsync(t => t.Token == tokenHash);
        }

        public async Task UpdateAsync(RefreshToken token)
        {
            _context.Update(token);
            await _context.SaveChangesAsync();
        }

    }
}
