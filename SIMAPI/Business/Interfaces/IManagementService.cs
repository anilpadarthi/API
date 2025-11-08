using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models;

namespace SIMAPI.Business.Interfaces
{
    public interface IManagementService
    {

        Task<CommonResponse> CreateWhatsAppNotificationRequestAsync(WhatsAppRequestDto request);
        Task<CommonResponse> GetUserSalaryTransactionAsync(int userSalaryTransactionID);
        Task<CommonResponse> GetUserSalaryTransactionsAsync(int userId, DateTime date);
        Task<CommonResponse> CreateUserSalaryTransactionAsync(UserSalaryTransaction request);
        Task<CommonResponse> UpdateUserSalaryTransactionAsync(UserSalaryTransaction request);
        Task<CommonResponse> DeleteUserSalaryTransactionAsync(int userSalaryTransactionID);
    }
}
