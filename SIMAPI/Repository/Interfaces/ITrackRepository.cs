using SIMAPI.Data.Dto;
using SIMAPI.Data.Models.Tracking;

namespace SIMAPI.Repository.Interfaces
{
    public interface ITrackRepository : IRepository
    {
        Task<IEnumerable<TrackReportModel>> GetTrackReportAsync(GetReportRequest request);
        Task<IEnumerable<UserTrackDataModel>> GetUserTrackDataReportAsync(GetReportRequest request);
        Task<IEnumerable<DailyReportModel>> GetDailyGivenReportAsync(GetReportRequest request);
        Task<IEnumerable<AreaVisitedModel>> GetAreasVisitedReportAsync(GetReportRequest request);
        Task<IEnumerable<ShopVisitedModel>> GetShopsVisitedReportAsync(GetReportRequest request);
        Task<List<dynamic>> GetShopsSimsGivenReportAsync(GetReportRequest request);
        Task<IEnumerable<LatLongInfoModel>> GetLatLongReportAsync(GetReportRequest request);
        Task<IEnumerable<UserTrackDataModel>> DownloadTrackAsync(GetReportRequest request);
    }
}
