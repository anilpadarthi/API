using AutoMapper;
using SIMAPI.Business.Helper;
using SIMAPI.Business.Helper.PDF;
using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models;
using SIMAPI.Repository.Interfaces;
using System.Net;

namespace SIMAPI.Business.Services
{
    public class CommissionStatementService : ICommissionStatementService
    {
        private readonly ICommissionStatementRepository _commissionStatementRepository;
        private readonly IMapper _mapper;
        public CommissionStatementService(ICommissionStatementRepository commissionStatementRepository, IMapper mapper)
        {
            _commissionStatementRepository = commissionStatementRepository;
            _mapper = mapper;
        }

        public async Task<CommonResponse> GetCommissionListAsync(GetReportRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _commissionStatementRepository.GetCommissionListAsync(request);
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
                response = response.HandleException(ex);
            }
            return response;
        }

        public async Task<byte[]> DownloadPDFStatementReportAsync(GetReportRequest request)
        {
            CommonResponse response = new CommonResponse();
            byte[] result = null;
            try
            {
                CommissionStatementPDF commissionStatementPDF = new CommissionStatementPDF();
                result = await commissionStatementPDF.GeneratePDFStatement(_commissionStatementRepository, request);
                if (result != null && result.Length > 0)
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
                response = response.HandleException(ex);
            }
            return result;
        }

        public async Task<byte[]> DownloadVATStatementReportAsync(GetReportRequest request)
        {
            CommonResponse response = new CommonResponse();
            byte[] result = null;
            try
            {
                CommissionStatementPDF commissionStatementPDF = new CommissionStatementPDF();
                result = await commissionStatementPDF.GenerateVATPDFStatement(_commissionStatementRepository, request);
                if (result != null && result.Length > 0)
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
                response = response.HandleException(ex);
            }
            return result;
        }


    }
}
