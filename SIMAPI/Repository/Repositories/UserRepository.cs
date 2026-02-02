using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SIMAPI.Business.Enums;
using SIMAPI.Data;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models;
using SIMAPI.Data.Models.Login;
using SIMAPI.Repository.Interfaces;

namespace SIMAPI.Repository.Repositories
{
    public class UserRepository : Repository, IUserRepository
    {
        public UserRepository(SIMDBContext context) : base(context)
        {
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Set<User>()
                    .ToListAsync();
        }

        public async Task<UserDetails?> GetUserDetailsAsync(int userId)
        {
            UserDetails userDetails = new UserDetails();
            userDetails.user = await _context.Set<User>()
                    .Where(w => w.UserId == userId)
                    .FirstOrDefaultAsync();

            userDetails.userDocuments = await GetUserDocumentsAsync(userId);

            userDetails.userSalarySettings = await _context.Set<UserSalarySetting>()
                    .Where(w => w.UserId == userId && w.IsActive == true)
                    .FirstOrDefaultAsync();

            return userDetails;
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _context.Set<User>()
                    .Where(w => w.UserId == userId)
                    .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<UserDocument>> GetUserDocumentsAsync(int userId)
        {
            return await _context.Set<UserDocument>()
                .Where(w => w.UserId == userId)
                    .ToListAsync();
        }

        public async Task<User?> GetUserByNameAsync(string name)
        {
            return await _context.Set<User>()
                .Where(w => w.UserName.ToUpper() == name.ToUpper())
                .Where(w => w.Status == (int)EnumStatus.Active)
                .FirstOrDefaultAsync();
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Set<User>()
                .Where(w => w.Email.ToUpper() == email.ToUpper())
                .Where(w => w.Status == (int)EnumStatus.Active)
                .FirstOrDefaultAsync();
        }


        public async Task<PasswordResetToken?> GetPasswordResetToken(string token)
        {
            return await _context.Set<PasswordResetToken>()
                .Where(w => w.Token.ToUpper() == token.ToUpper())
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<User>> GetUsersByPagingAsync(GetPagedSearch request)
        {
            var query = _context.Set<User>()
                        .Include(i => i.UserRole)
                        .AsQueryable();
            query = query.Where(w => w.Status != (int)EnumStatus.Deleted);
            if (!string.IsNullOrEmpty(request.searchText))
            {
                if (int.TryParse(request.searchText, out int userId))
                {
                    // If numeric → search by ID
                    query = query.Where(w => w.UserId == userId);
                }
                else
                {
                    // Otherwise → search by name
                    query = query
                        .Where(w => w.UserName.Contains(request.searchText)
                               || w.Email.Contains(request.searchText));
                }
            }
            if (request.userRoleId.HasValue)
            {
                query = query.Where(w => w.UserRoleId == request.userRoleId);
            }

            var result = await query
                .OrderBy(o => o.UserName)
                .Skip((request.pageNo - 1) * request.pageSize)
                .Take(request.pageSize)
                .ToListAsync();

            return result;
        }

        public async Task<int> GetTotalUserCountAsync(GetPagedSearch request)
        {
            var query = _context.Set<User>().AsQueryable();

            query = query.Where(w => w.Status != (int)EnumStatus.Deleted);
            if (!string.IsNullOrEmpty(request.searchText))
            {
                if (int.TryParse(request.searchText, out int userId))
                {
                    // If numeric → search by ID
                    query = query.Where(w => w.UserId == userId);
                }
                else
                {
                    // Otherwise → search by name
                    query = query
                        .Where(w => w.UserName.Contains(request.searchText)
                               || w.Email.Contains(request.searchText));
                }
            }
            if (request.userRoleId.HasValue)
            {
                query = query.Where(w => w.UserRoleId == request.userRoleId);
            }
            return await query.CountAsync();
        }

        public async Task<LoggedInUserDto?> GetUserDetailsAsync(string email, string password)
        {
            var result = await _context.Set<User>()
               .Include(i => i.UserRole)
                        .Where(w => (w.Email == email || w.UserName == email) && w.Password == password && w.Status == (int)EnumStatus.Active)
                        .FirstOrDefaultAsync();
            if (result != null)
            {
                LoggedInUserDto loggedInUserDto = new LoggedInUserDto()
                {
                    userId = result.UserId,
                    userName = result.UserName,
                    userRoleId = result.UserRoleId,
                    userRole = result.UserRole,
                    email = result.Email,
                    userImage = result.UserImage,
                    firstName = result.FirstName,
                    lastName = result.LastName,
                    designation = result.Designation,
                    mobile = result.Mobile,
                    doj = result.DOJ
                };
                return loggedInUserDto;
            }
            return null;
        }

        public async Task<LoggedInUserDto?> GetUserDetailsByUserIdAsync(int userId)
        {
            var result = await _context.Set<User>()
               .Include(i => i.UserRole)
                        .Where(w => w.UserId == userId)
                        .FirstOrDefaultAsync();
            if (result != null)
            {
                LoggedInUserDto loggedInUserDto = new LoggedInUserDto()
                {
                    userId = result.UserId,
                    userName = result.UserName,
                    userRoleId = result.UserRoleId,
                    userRole = result.UserRole,
                    email = result.Email,
                    userImage = result.UserImage,
                    firstName = result.FirstName,
                    lastName = result.LastName,
                    designation = result.Designation,
                    mobile = result.Mobile,
                    doj = result.DOJ
                };
                return loggedInUserDto;
            }
            return null;
        }

        public async Task<LoggedInUserDto?> GetRetailerUserDetailsAsync(string email, string password)
        {
            var query = _context.Set<Shop>()
                        .Where(w => w.Password == password && w.Status == (int)EnumStatus.Active);

            if (int.TryParse(email, out int shopId))
            {
                query = query.Where(w => w.ShopId == shopId || w.OldShopId == shopId);
            }
            else
            {
                query = query.Where(w => w.ShopName == email || w.ShopEmail == email);
            }



            var result = await query.FirstOrDefaultAsync();
            if (result != null)
            {
                UserRole role = new UserRole()
                {
                    UserRoleId = (int)EnumUserRole.Retailer,
                    RoleName = "Retailer"
                };
                LoggedInUserDto loggedInUserDto = new LoggedInUserDto()
                {
                    userId = result.ShopId,
                    userName = result.ShopName,
                    userRoleId = (int)EnumUserRole.Retailer,
                    userRole = role,
                    email = result.ShopName,
                    userImage = result.Image
                };
                return loggedInUserDto;
            }
            return null;
        }

        public async Task<IEnumerable<UserRoleOption>> GetUserRoleOptionsAsync(int userRoleId)
        {
            var sqlParameters = new[]
            {
                new SqlParameter("@userRoleId", userRoleId)
            };
            return await ExecuteStoredProcedureAsync<UserRoleOption>("exec dbo.Get_User_Options @userRoleId", sqlParameters);
        }

        public async Task<IEnumerable<AllocateAgentDetails>> GetAllAgentsToAllocateAsync()
        {
            return await ExecuteStoredProcedureAsync<AllocateAgentDetails>("exec [dbo].[Get_All_Agents_To_Allocate]");
        }

        public async Task<UserMap> GetAgentMapByAgentIdAsync(int agentId)
        {
            return await _context.Set<UserMap>()
                .Where(w => w.UserId == agentId && w.IsActive == true)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<UserAllocationHistory>> ViewUserAllocationHistorySync(int userId)
        {
            var paramList = new[]
            {
                    new SqlParameter("@userId", userId),
            };
            return await ExecuteStoredProcedureAsync<UserAllocationHistory>("exec [dbo].[Get_User_Allocate_History] @userId", paramList);
        }


        public async Task<IEnumerable<string>> GetUserNotificationsAsync(int userId)
        {
            var paramList = new[]
           {
                    new SqlParameter("@userId", userId),
            };
            return await ExecutePrimitiveStoredProcedureAsync<string>("exec [dbo].[Get_User_Notifications] @userId", paramList);

        }

        public async Task<UserSalarySetting?> GetUserSalarySettingAsync(int userId)
        {
            return await _context.Set<UserSalarySetting>()
                .FirstOrDefaultAsync(x => x.UserId == userId
                    && x.IsActive == true);
        }

    }
}
