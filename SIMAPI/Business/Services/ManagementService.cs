using AutoMapper;
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
            try
            {
                WhatsAppRequest obj = new WhatsAppRequest();
                obj.RequestType = request.RequestType;
                obj.FromDate = request.FromDate;
                obj.ToDate = request.ToDate?? request.FromDate;
                obj.Status = "Pending";
                obj.CreatedDate = DateTime.Now;
                obj.UserId = request.UserId;
                obj.UserType = request.UserType;

                _managementRepository.Add(obj);
                await _managementRepository.SaveChangesAsync();

                response = Utility.CreateResponse("Successfully created.", HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _managementRepository);
            }
            return response;
        }

        public async Task<CommonResponse> GetUserSalaryTransactionAsync(int userSalaryTransactionID)
        {
            CommonResponse response = new CommonResponse();
            try
            {


                var result = await _managementRepository.GetUserSalaryTransactionAsync(userSalaryTransactionID);

                if (result != null)
                {
                    response = Utility.CreateResponse(result, HttpStatusCode.OK);
                }
                else
                {
                    response = Utility.CreateResponse("Not found", HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _managementRepository);
            }
            return response;
        }

        public async Task<CommonResponse> CreateUserSalaryTransactionAsync(UserSalaryTransaction request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                request.CreatedDate = DateTime.Now;
                request.IsActive = 1;

                _managementRepository.Add(request);
                await _managementRepository.SaveChangesAsync();

                response = Utility.CreateResponse("Created successfully", HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _managementRepository);
            }
            return response;
        }

        public async Task<CommonResponse> UpdateUserSalaryTransactionAsync(UserSalaryTransaction request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
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
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _managementRepository);
            }
            return response;
        }

        public async Task<CommonResponse> DeleteUserSalaryTransactionAsync(int userSalaryTransactionID)
        {
            CommonResponse response = new CommonResponse();
            try
            {

                var result = await _managementRepository.GetUserSalaryTransactionAsync(userSalaryTransactionID);

                if (result != null)
                {
                    result.IsActive = 0;
                    await _managementRepository.SaveChangesAsync();

                    response = Utility.CreateResponse("Deleted successfully", HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _managementRepository);
            }
            return response;
        }

        public async Task<CommonResponse> GetUserSalaryTransactionsAsync(int userId, DateTime date)
        {
            CommonResponse response = new CommonResponse();
            try
            {

                var result = await _managementRepository.GetUserSalaryTransactionsAsync(userId, date);
                if (result != null)
                {
                    response = Utility.CreateResponse(result, HttpStatusCode.OK);
                }
                else
                {
                    response = Utility.CreateResponse("Not found", HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _managementRepository);
            }
            return response;
        }

    }
}
