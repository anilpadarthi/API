﻿using SIMAPI.Data.Dto;
using SIMAPI.Data.Models.Report;
using SIMAPI.Data.Models.Report.InstantReport;
using System.Data;

namespace SIMAPI.Repository.Interfaces
{
    public interface IReportRepository : IRepository
    {
        Task<IEnumerable<MonthlyActivationModel>> GetMonthlyActivationsAsync(GetReportRequest request);
        Task<List<dynamic>> GetMonthlyHistoryActivationsAsync(GetReportRequest request);
        Task<IEnumerable<DailyGivenCountModel>> GetDailyGivenCountAsync(GetReportRequest request);
        Task<IEnumerable<NetworkUsageModel>> GetNetworkUsageReportAsync(GetReportRequest request);
        Task<IEnumerable<KPITargetReportModel>> GetKPITargetReportAsync(GetReportRequest request);




        Task<IEnumerable<MonthlyUserActivationModel>> GetMonthlyUserActivationsAsync(GetReportRequest request);
        Task<IEnumerable<MonthlyAreaActivationModel>> GetMonthlyAreaActivationsAsync(GetReportRequest request);
        Task<IEnumerable<MonthlyShopActivationModel>> GetMonthlyShopActivationsAsync(GetReportRequest request);
       
        Task<IEnumerable<InstantActivationReportModel>> GetInstantActivationReportAsync(GetReportRequest request);
      
        
        
        Task<IEnumerable<SalaryReportModel>> GetSalaryReportAsync(GetReportRequest request);
        
    }
}