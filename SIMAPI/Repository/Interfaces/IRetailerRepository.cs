using SIMAPI.Data.Dto;
using SIMAPI.Data.Models.Retailer;

namespace SIMAPI.Repository.Interfaces
{
    public interface IRetailerRepository : IRepository
    {
        Task<IEnumerable<ActivationModel>> GetActvationsAsync(GetReportRequest request);
        Task<IEnumerable<ShopCommissionModel>> GetCommissionsAsync(GetReportRequest request);
        Task<IEnumerable<StockVsConnectionModel>> GetStockVsConnectionsAsync(GetReportRequest request);
    }
}
