using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIMAPI.Business.Services
{
    public class CommunicationService : ICommunicationService
    {
        Task<CommonResponse> ICommunicationService.SendEmailAsync(GetReportRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
