using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models;

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
        Task<User?> GetUserDetailsAsync(string email, string password);
        Task<IEnumerable<UserRoleOption>> GetUserRoleOptionsAsync(int userRoleId);

    }

}
