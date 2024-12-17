using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Models;

namespace SIMAPI.Business.Services
{
    public class TransactionService : ITransactionService
    {
        Task<CommonResponse> ITransactionService.AddSalaryAmount(GetReportRequest request)
        {
            throw new NotImplementedException();
        }

        Task<CommonResponse> ITransactionService.UpdateSalaryAmount(GetReportRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
