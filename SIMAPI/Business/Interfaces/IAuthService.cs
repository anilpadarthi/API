using SIMAPI.Data.Models;

namespace SIMAPI.Business.Interfaces
{
    public interface IAuthService
    {

        Task<CommonResponse> ValidateUser(string email, string password);
    }
}
