using SIMAPI.Data.Dto;
using SIMAPI.Data.Models;

namespace SIMAPI.Business.Interfaces
{
    public interface ITrackService
    {
        Task<CommonResponse> GetTrackReportAsync(GetReportRequest request);
        Task<CommonResponse> GetUserTrackDataReportAsync(GetReportRequest request);
        Task<CommonResponse> GetDailyGivenReportAsync(GetReportRequest request);
        Task<CommonResponse> GetAreasVisitedReportAsync(GetReportRequest request);
        Task<CommonResponse> GetShopsVisitedReportAsync(GetReportRequest request);
        Task<CommonResponse> GetShopsSimsGivenReportAsync(GetReportRequest request);
        Task<CommonResponse> GetLatLongReportAsync(GetReportRequest request);
        Task<CommonResponse> LogUserTrackAsync(UserTrackDto request);
        Task<CommonResponse> DownloadAttendaceAsync(string date);

    }
}
