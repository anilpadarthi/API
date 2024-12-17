using SIMAPI.Data.Dto;
using SIMAPI.Data.Models;

namespace SIMAPI.Business.Interfaces
{
    public interface ITransactionService
    {

        Task<CommonResponse> AddSalaryAmount(GetReportRequest request);
        Task<CommonResponse> UpdateSalaryAmount(GetReportRequest request);
    }
}
