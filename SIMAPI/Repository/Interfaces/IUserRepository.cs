using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models;
using SIMAPI.Data.Models.Login;

namespace SIMAPI.Repository.Interfaces
{
    public interface IUserRepository : IRepository
    {
        Task<User?> GetUserByIdAsync(int userId);
        Task<UserDetails?> GetUserDetailsAsync(int userId);
        Task<IEnumerable<UserDocument>> GetUserDocumentsAsync(int userId);
        Task<User?> GetUserByNameAsync(string name);
        Task<User?> GetUserByEmailAsync(string email);
        
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<User>> GetUsersByPagingAsync(GetPagedSearch request);
        Task<int> GetTotalUserCountAsync(GetPagedSearch request);
        Task<LoggedInUserDto?> GetUserDetailsAsync(string email, string password);
        Task<LoggedInUserDto?> GetRetailerUserDetailsAsync(string email, string password);
        Task<IEnumerable<UserRoleOption>> GetUserRoleOptionsAsync(int userRoleId);
        Task<UserMap> GetAgentMapByAgentIdAsync(int agetnId);
        Task<IEnumerable<AllocateAgentDetails>> GetAllAgentsToAllocateAsync();
        Task<IEnumerable<UserAllocationHistory>> ViewUserAllocationHistorySync(int userId);
        Task<IEnumerable<string>> GetUserNotificationsAsync(int userId);
        Task<UserSalarySetting?> GetUserSalarySettingAsync(int userId);

        Task<PasswordResetToken?> GetPasswordResetToken(string token);

    }

}
