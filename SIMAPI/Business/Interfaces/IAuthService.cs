using SIMAPI.Data.Entities;
using SIMAPI.Data.Models;
using SIMAPI.Data.Models.Login;

namespace SIMAPI.Business.Interfaces
{
    public interface IAuthService
    {

        Task<CommonResponse> ValidateUser(string email, string password);
        Task<CommonResponse> ValidateUser(LoginRequest request);
        Task<LoggedInUserDto?> GetUserDetailsAsync(string email, string password);
        Task<LoggedInUserDto?> GetRetailerUserDetailsAsync(string email, string password);
    }
}
