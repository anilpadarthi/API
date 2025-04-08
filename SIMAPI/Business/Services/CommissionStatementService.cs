using AutoMapper;
using SIMAPI.Business.Helper;
using SIMAPI.Business.Helper.PDF;
using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models;
using SIMAPI.Data.Models.CommissionStatement;
using SIMAPI.Repository.Interfaces;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        public async Task<CommonResponse> GetCommissionHistoryDetailsAsync(int shopCommissionHistoryId)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _commissionStatementRepository.GetCommissionHistoryDetailsAsync(shopCommissionHistoryId);
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

        public async Task<CommonResponse> OptInForShopCommissionAsync(int shopCommissionHistoryId, string optInType, int userId)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var commissionHistoryDetails = await _commissionStatementRepository.GetCommissionHistoryDetailsAsync(shopCommissionHistoryId);
                if (commissionHistoryDetails != null)
                {
                    if (commissionHistoryDetails.IsRedemed == false)
                    {
                        commissionHistoryDetails.IsRedemed = true;
                        commissionHistoryDetails.OptInType = optInType;
                        if (optInType == "Wallet")
                        {
                            ShopWalletHistory shopWalletHistory = new ShopWalletHistory();
                            shopWalletHistory.Amount = commissionHistoryDetails.CommissionAmount ?? 0;
                            shopWalletHistory.TransactionType = "Credit";
                            shopWalletHistory.ReferenceNumber = Convert.ToString(commissionHistoryDetails.ShopCommissionHistoryId);
                            shopWalletHistory.CommissionDate = commissionHistoryDetails.CommissionDate;
                            shopWalletHistory.ShopId = commissionHistoryDetails.ShopId;
                            shopWalletHistory.UserId = userId;
                            shopWalletHistory.WalletType = "Commission";
                            shopWalletHistory.TransactionDate = DateTime.Now;
                            shopWalletHistory.IsActive = true;
                            shopWalletHistory.Comments = "Commission credited for the month of " + commissionHistoryDetails.CommissionDate.ToString("MMM, yy");
                            _commissionStatementRepository.Add(shopWalletHistory);

                        }
                        await _commissionStatementRepository.SaveChangesAsync();
                        response = Utility.CreateResponse(commissionHistoryDetails, HttpStatusCode.OK);
                    }
                    else
                    {
                        response = Utility.CreateResponse("You can not opt in, It has redeemed", HttpStatusCode.Conflict);
                    }
                }
                else
                {
                    response = Utility.CreateResponse("record does not exist", HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex);
            }
            return response;
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

        public async Task<IEnumerable<ExportCommissionList>> ExportCommissionChequeExcelAsync(GetReportRequest request)
        {
            return await _commissionStatementRepository.ExportCommissionChequeExcelAsync(request);

        }

        public async Task<byte[]> DownloadPDFStatementReportAsync(GetReportRequest request)
        {
            CommonResponse response = new CommonResponse();
            byte[] result = null;
            try
            {
                CommissionStatementPDF commissionStatementPDF = new CommissionStatementPDF();
                result = await commissionStatementPDF.GeneratePDFStatement(_commissionStatementRepository, request, true);
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

        public async Task<CommonResponse> HideBonusAsync(int shopCommissionHistoryId, bool isDisplayBonus)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _commissionStatementRepository.GetShopBonusHistoryByReferenceNumber(shopCommissionHistoryId);
                if (result != null)
                {
                    result.IsActive = isDisplayBonus;
                    await _commissionStatementRepository.SaveChangesAsync();
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


    }
}
