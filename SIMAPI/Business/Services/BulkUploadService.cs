using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIMAPI.Business.Services
{
    public class BulkUploadService : IBulkUploadService
    {
        Task<CommonResponse> IBulkUploadService.UploadFile(GetReportRequest request)
        {
            throw new NotImplementedException();
        }

        Task<CommonResponse> IBulkUploadService.UploadTargetFile(GetReportRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
