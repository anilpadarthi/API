using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SIMAPI.Business.Enums;
using SIMAPI.Data;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models;
using SIMAPI.Data.Models.OrderListModels;
using SIMAPI.Repository.Interfaces;

namespace SIMAPI.Repository.Repositories
{
    public class ManagementRepository : Repository, IManagementRepository
    {
        public ManagementRepository(SIMDBContext context) : base(context)
        {
        }



        public async Task<UserSalaryTransaction?> GetUserSalaryTransactionAsync(int id)
        {
            return await _context.Set<UserSalaryTransaction>()
                .Where(w => w.UserSalaryTransactionID == id)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<UserSalaryTransaction>> GetUserSalaryTransactionsAsync(int userId, DateTime date)
        {
            return await _context.Set<UserSalaryTransaction>()
                .Where(w => w.UserId == userId && date >= w.TransactionDate && date.AddMonths(1) < w.TransactionDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<AccessoriesOutstandingBalanceModel>> OutStandingAccessoriesReportAsync(GetReportRequest request)
        {
            var sqlParameters = new[]
             {
                new SqlParameter("@date", request.fromDate),
            };
            return await ExecuteStoredProcedureAsync<AccessoriesOutstandingBalanceModel>("exec [dbo].[Get_OutStandingAccessories_Report] @date", sqlParameters);


        }

        public async Task<SalaryCommissionConfiguration?> GetCommissionConfigurationAsync(SalaryCommissionConfiguration model)
        {
            return await _context.Set<SalaryCommissionConfiguration>()
                .Where(w=>w.UserId == model.UserId && w.PayslipDate == model.PayslipDate)
                .FirstOrDefaultAsync();
        }

        public async Task<Configuration?> GetConfigurationAsync()
        {
            return await _context.Set<Configuration>()
                .FirstOrDefaultAsync();
        }


    }
}
