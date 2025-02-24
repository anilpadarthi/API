using SIMAPI.Data.Dto;
using SIMAPI.Data.Models;

namespace SIMAPI.Business.Interfaces
{
    public interface IManagementService
    {

        Task<CommonResponse> CreateWhatsAppNotificationRequestAsync(WhatsAppRequestDto request);
    }
}
