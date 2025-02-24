using AutoMapper;
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
                obj.ToDate = request.ToDate;
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
                response = response.HandleException(ex);
            }
            return response;
        }

    }
}
