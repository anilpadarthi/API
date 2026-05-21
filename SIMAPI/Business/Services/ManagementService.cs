using AutoMapper;
using Azure.Core;
using Microsoft.Data.SqlClient;
using SIMAPI.Business.Helper;
using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models;
using SIMAPI.Repository.Interfaces;
using System.Net;

namespace SIMAPI.Business.Services
{
    public class ManagementService : IManagementService
    {
        private readonly IManagementRepository _managementRepository;
        private readonly IMapper _mapper;
        public ManagementService(IManagementRepository managementRepository, IMapper mapper)
        {
            _managementRepository = managementRepository;
            _mapper = mapper;
        }
        public async Task<CommonResponse> CreateWhatsAppNotificationRequestAsync(WhatsAppRequestDto request)
        {
            CommonResponse response = new CommonResponse();

            WhatsAppRequest obj = new WhatsAppRequest();
            obj.RequestType = request.RequestType;
            obj.FromDate = request.FromDate;
            obj.ToDate = request.ToDate ?? request.FromDate;
            obj.Status = "Pending";
            obj.CreatedDate = DateTime.Now;
            obj.UserId = request.UserId;
            obj.UserType = request.UserType;

            _managementRepository.Add(obj);
            await _managementRepository.SaveChangesAsync();

            response = Utility.CreateResponse("Successfully created.", HttpStatusCode.OK);


            return response;
        }

        public async Task<CommonResponse> GetUserSalaryTransactionAsync(int userSalaryTransactionID)
        {
            CommonResponse response = new CommonResponse();



            var result = await _managementRepository.GetUserSalaryTransactionAsync(userSalaryTransactionID);

            if (result != null)
            {
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            else
            {
                response = Utility.CreateResponse("Not found", HttpStatusCode.NotFound);
            }

            return response;
        }

        public async Task<CommonResponse> CreateUserSalaryTransactionAsync(UserSalaryTransaction request)
        {
            CommonResponse response = new CommonResponse();

            request.CreatedDate = DateTime.Now;
            request.IsActive = 1;

            _managementRepository.Add(request);
            await _managementRepository.SaveChangesAsync();

            response = Utility.CreateResponse("Created successfully", HttpStatusCode.Created);

            return response;
        }

        public async Task<CommonResponse> UpdateUserSalaryTransactionAsync(UserSalaryTransaction request)
        {
            CommonResponse response = new CommonResponse();

            if (!request.UserSalaryTransactionID.HasValue)
            {
                response = Utility.CreateResponse("Invalid request - missing UserSalaryTransactionID", HttpStatusCode.BadRequest);
                return response;
            }
            var result = await _managementRepository.GetUserSalaryTransactionAsync(request.UserSalaryTransactionID.Value);
            if (result != null)
            {
                result.TransactionDate = request.TransactionDate;
                result.Comments = request.Comments;
                result.Amount = request.Amount;
                result.Type = request.Type;
                await _managementRepository.SaveChangesAsync();
            }

            response = Utility.CreateResponse("Updated successfully", HttpStatusCode.OK);

            return response;
        }

        public async Task<CommonResponse> DeleteUserSalaryTransactionAsync(int userSalaryTransactionID)
        {
            CommonResponse response = new CommonResponse();


            var result = await _managementRepository.GetUserSalaryTransactionAsync(userSalaryTransactionID);

            if (result != null)
            {
                result.IsActive = 0;
                await _managementRepository.SaveChangesAsync();

                response = Utility.CreateResponse("Deleted successfully", HttpStatusCode.OK);
            }

            return response;
        }

        public async Task<CommonResponse> GetUserSalaryTransactionsAsync(int userId, DateTime date)
        {
            CommonResponse response = new CommonResponse();


            var result = await _managementRepository.GetUserSalaryTransactionsAsync(userId, date);
            if (result != null)
            {
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            else
            {
                response = Utility.CreateResponse("Not found", HttpStatusCode.NotFound);
            }

            return response;
        }

        public async Task<CommonResponse> OutStandingAccessoriesReportAsync(GetReportRequest request)
        {
            CommonResponse response = new CommonResponse();

            var result = await _managementRepository.OutStandingAccessoriesReportAsync(request);
            if (result != null)
            {
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            else
            {
                response = Utility.CreateResponse("Not found", HttpStatusCode.NotFound);
            }

            return response;

        }

        public async Task<CommonResponse> SaveCommissionConfigurationAsync(SalaryCommissionConfiguration model)
        {
            CommonResponse response = new CommonResponse();
            var configurationModel = await _managementRepository.GetCommissionConfigurationAsync(model);
            if (configurationModel != null)
            {
                configurationModel.IsCommissionBasedOnCutoffDate = model.IsCommissionBasedOnCutoffDate;
                configurationModel.IsCommissionBasedOnCollectedAmount = model.IsCommissionBasedOnCollectedAmount;
            }
            else
            {
                _managementRepository.Add(model);
            }
            await _managementRepository.SaveChangesAsync();
            response = Utility.CreateResponse("Saved Successfully", HttpStatusCode.OK);
            return response;
        }

        public async Task<CommonResponse> GetCommissionConfigurationAsync(SalaryCommissionConfiguration model)
        {
            CommonResponse response = new CommonResponse();

            var result = await _managementRepository.GetCommissionConfigurationAsync(model);
            if (result != null)
            {
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            else
                response = Utility.CreateResponse("Not found", HttpStatusCode.NotFound);

            return response;
        }

        public async Task<CommonResponse> SaveConfigurationAsync(string chequeCutOffDay)
        {
            CommonResponse response = new CommonResponse();
            var configurationModel = await _managementRepository.GetConfigurationAsync();
            configurationModel.chequeCutOffDay = chequeCutOffDay;
            await _managementRepository.SaveChangesAsync();
            response = Utility.CreateResponse("Saved Successfully", HttpStatusCode.OK);
            return response;
        }

        public async Task<CommonResponse> GetConfigurationAsync()
        {
            CommonResponse response = new CommonResponse();

            var result = await _managementRepository.GetConfigurationAsync();
            if (result != null)
            {
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            else
                response = Utility.CreateResponse("Not found", HttpStatusCode.NotFound);


            return response;
        }

    }
}
