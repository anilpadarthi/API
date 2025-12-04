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

            var refreshToken = await _context.Set<RefreshToken>()                
               .FirstOrDefaultAsync(t => t.TokenHash == tokenHash);
            if(refreshToken != null)
            {
                var user = await _context.Set<User>()
                    .Include(u=>u.UserRole)
               .FirstOrDefaultAsync(t => t.UserId == refreshToken.UserId);
                if(user != null)
                {
                    refreshToken.User = new LoggedInUserDto()
                    {
                        userId = user.UserId,
                        userName = user.UserName,
                        email = user.Email,
                        userRoleId = user.UserRoleId,
                        userImage = user.UserImage,
                        userRole = user.UserRole,
                        firstName = user.FirstName,
                        lastName = user.LastName,
                        mobile = user.Mobile,
                        doj = user.DOJ,
                        designation = user.Designation

                    };
                }
                return refreshToken;
            }
            return null;
        }

    }
}
