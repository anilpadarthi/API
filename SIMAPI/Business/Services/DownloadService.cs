using AutoMapper;
using CsvHelper;
using SIMAPI.Business.Enums;
using SIMAPI.Business.Helper;
using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Models.Report.InstantReport;
using SIMAPI.Repository.Interfaces;
using System.Globalization;

namespace SIMAPI.Business.Services
{
    public class DownloadService : IDownloadService
    {
        private readonly IReportRepository _reportRepository;
        private readonly IMapper _mapper;
        public DownloadService(IReportRepository reportRepository, IMapper mapper)
        {
            _reportRepository = reportRepository;
            _mapper = mapper;
        }

        public async Task<Stream?> DownloadInstantActivationListAsync(GetReportRequest request)
        {
            var result = await _reportRepository.GetInstantActivationReportAsync(request);
            var stream = new MemoryStream();

            using (var sw = new StreamWriter(stream, leaveOpen: true))
            {
                using (var cw = new CsvWriter(sw, CultureInfo.InvariantCulture, true))
                {
                    cw.Context.RegisterClassMap<ExportInstantActivations>();
                    cw.WriteRecords(result);
                    sw.Flush();
                }
            }
            stream.Position = 0;

            return stream;
        }

        public async Task<Stream?> DownloadDailyActivtionsAsync(GetReportRequest request)
        {
            var result = await _reportRepository.DownloadDailyActivtionsAsync(request);
            var stream = ExcelUtility.ConvertDataToExcelFormat<DownloadDailyActivationModel>(result.ToList());

            return stream;
        }

        public async Task<Stream?> DownloadActivtionAnalysisReportAsync(GetReportRequest request)
        {
            if (request.userId.HasValue && request.areaId.HasValue && request.shopId.HasValue)
            {
                request.filterMode = "By Shop";
                request.filterId = request.shopId;
            }

            else if (request.userId.HasValue && request.areaId.HasValue)
            {
                request.filterMode = "By Area";
                request.filterId = request.areaId;
            }

            else if (request.userId.HasValue || request.userRoleId == (int)EnumUserRole.Agent)
            {
                request.filterMode = "By Agent";
                request.filterType = "Agent";
                request.filterId = request.userRoleId == (int)EnumUserRole.Agent ? request.loggedInUserId : request.userId;
                request.userId = request.loggedInUserId;
            }
            else if (request.managerId.HasValue || request.userRoleId == (int)EnumUserRole.Manager)
            {
                request.filterMode = "All";
                request.filterType = "Manager";
                request.filterId = request.userRoleId == (int)EnumUserRole.Manager ? request.loggedInUserId : request.managerId;
                request.userId = request.loggedInUserId;
            }
            else
            {
                request.filterMode = "All";
            }
            var result = await _reportRepository.DownloadActivtionAnalysisReportAsync(request);
            var stream = ExcelUtility.ConvertDynamicDataToExcelFormatWithColours<dynamic>(result.ToList());

            return stream;
        }        

    }
}
