﻿using SIMAPI.Data.Dto;
using SIMAPI.Data.Models;

namespace SIMAPI.Business.Interfaces
{
    public interface IBulkUploadService
    {

        Task<CommonResponse> UploadFile(GetReportRequest request);
        Task<CommonResponse> UploadTargetFile(GetReportRequest request);
    }
}