using AutoMapper;
using SIMAPI.Business.Helper;
using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models;
using SIMAPI.Data.Models.Tracking;
using SIMAPI.Repository.Interfaces;
using System.Net;
using System.Text.Json;

namespace SIMAPI.Business.Services
{
    public class TrackService : ITrackService
    {
        private readonly ITrackRepository _trackRepository;
        private readonly IMapper _mapper;
        public TrackService(ITrackRepository trackRepository, IMapper mapper)
        {
            _trackRepository = trackRepository;
            _mapper = mapper;
        }
        public async Task<CommonResponse> GetAreasVisitedReportAsync(GetReportRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _trackRepository.GetAreasVisitedReportAsync(request);
                if (result != null)
                {
                    response = Utility.CreateResponse(result, HttpStatusCode.OK);
                }
                else
                {
                    response = Utility.CreateResponse("report does not exist", HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _trackRepository);
            }
            return response;
        }

        public async Task<CommonResponse> GetDailyGivenReportAsync(GetReportRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _trackRepository.GetDailyGivenReportAsync(request);
                if (result != null)
                {
                    response = Utility.CreateResponse(result, HttpStatusCode.OK);
                }
                else
                {
                    response = Utility.CreateResponse("report does not exist", HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _trackRepository);
            }
            return response;
        }

        public async Task<CommonResponse> GetShopsSimsGivenReportAsync(GetReportRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _trackRepository.GetShopsSimsGivenReportAsync(request);
                if (result != null)
                {
                    response = Utility.CreateResponse(result, HttpStatusCode.OK);
                }
                else
                {
                    response = Utility.CreateResponse("report does not exist", HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _trackRepository);
            }
            return response;
        }

        public async Task<CommonResponse> GetShopsVisitedReportAsync(GetReportRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _trackRepository.GetShopsVisitedReportAsync(request);
                if (result != null)
                {
                    response = Utility.CreateResponse(result, HttpStatusCode.OK);
                }
                else
                {
                    response = Utility.CreateResponse("report does not exist", HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _trackRepository);
            }
            return response;
        }

        public async Task<CommonResponse> GetTrackReportAsync(GetReportRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _trackRepository.GetTrackReportAsync(request);
                if (result != null)
                {
                    response = Utility.CreateResponse(result, HttpStatusCode.OK);
                }
                else
                {
                    response = Utility.CreateResponse("report does not exist", HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                var json = JsonSerializer.Serialize(request);
                response = response.HandleException(ex, _trackRepository, json);
            }
            return response;
        }

        public async Task<CommonResponse> GetUserTrackDataReportAsync(GetReportRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _trackRepository.GetUserTrackDataReportAsync(request);
                if (result != null)
                {
                    response = Utility.CreateResponse(result, HttpStatusCode.OK);
                }
                else
                {
                    response = Utility.CreateResponse("report does not exist", HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                var json = JsonSerializer.Serialize(request);
                response = response.HandleException(ex, _trackRepository, json);
            }
            return response;
        }

        public async Task<CommonResponse> GetLatLongReportAsync(GetReportRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _trackRepository.GetLatLongReportAsync(request);
                if (result != null)
                {
                    response = Utility.CreateResponse(result, HttpStatusCode.OK);
                }
                else
                {
                    response = Utility.CreateResponse("report does not exist", HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _trackRepository);
            }
            return response;
        }

        public async Task<CommonResponse> LogUserTrackAsync(UserTrackDto request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var userTrackDbo = _mapper.Map<UserTrack>(request);
                _trackRepository.Add(userTrackDbo);
                await _trackRepository.SaveChangesAsync();
                response = Utility.CreateResponse(userTrackDbo, HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _trackRepository);
            }
            return response;
        }

        public async Task<CommonResponse> SaveAttendanceAsync(List<AttendanceDto> request)
        {
            CommonResponse response = new CommonResponse();

            try
            {
                foreach (var item in request)
                {
                    // Check if attendance already exists for user + date
                    var existing = await _trackRepository
                        .GetAttendanceAsync(item.UserId, item.DateOfAttendance);

                    if (existing != null)
                    {
                        // === UPDATE ===
                        existing.AttendanceType = item.AttendanceType ?? "";
                        existing.UpdatedDate = DateTime.Now;
                        existing.Comments = item.Comments ?? "";

                        _trackRepository.Update(existing);
                    }
                    else if (!string.IsNullOrEmpty(item.AttendanceType))
                    {
                        // === INSERT ===
                        var entity = new Attendance
                        {
                            UserId = item.UserId,
                            DateOfAttendance = item.DateOfAttendance,
                            AttendanceType = item.AttendanceType,
                            CreatedDate = DateTime.Now,
                            Comments = item.Comments ?? ""
                        };

                        _trackRepository.Add(entity);
                    }
                }

                await _trackRepository.SaveChangesAsync();

                response = Utility.CreateResponse("Attendance saved/updated successfully", HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _trackRepository);
            }

            return response;
        }


        public async Task<Stream> DownloadTrackAsync(GetReportRequest request)
        {
            var trackData = await _trackRepository.DownloadTrackAsync(request);
            var stream = ExcelUtility.ConvertDataToExcelFormat<UserTrackDataModel>(trackData.ToList());
            return stream;
        }
    }
}
