using DocumentFormat.OpenXml.Bibliography;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SIMAPI.Business.Enums;
using SIMAPI.Data;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models;
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


    }
}
