﻿using SIMAPI.Data.Dto;
using SIMAPI.Data.Models.CommissionStatement;

namespace SIMAPI.Repository.Interfaces
{
    public interface ICommissionStatementRepository : IRepository
    {
        Task<IEnumerable<CommissionListModel>> GetCommissionListAsync(GetReportRequest request);
        Task<IEnumerable<CommissionShopListModel>> GetShopListForCommission(GetReportRequest request);
        Task<IEnumerable<CommissionStatementModel?>> GetCommissionStatementAsync(GetReportRequest request);
    }
}