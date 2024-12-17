using SIMAPI.Data.Dto;
using SIMAPI.Data.Models;

namespace SIMAPI.Business.Interfaces
{
    public interface ICommunicationService
    {

        Task<CommonResponse> SendEmailAsync(GetReportRequest request);
    }
}
