using SIMAPI.Data.Dto;
using SIMAPI.Data.Models.Retailer;

namespace SIMAPI.Repository.Interfaces
{
    public interface IRetailerRepository : IRepository
    {
        Task<IEnumerable<ActivationModel>> GetActvationsAsync(GetReportRequest request);
        Task<IEnumerable<SimGivenDetailListModel>> GetSimGivenAsync(GetReportRequest request);
        Task<IEnumerable<ActivationDetaiListModel>> GetActivationDetaiListAsync(GetReportRequest request);
        Task<IEnumerable<RetailerCommissionListModel>> GetRetailerCommissionListAsync(GetReportRequest request);
        Task<List<dynamic>> GetStockVsConnectionsAsync(GetReportRequest request);
    }
}
