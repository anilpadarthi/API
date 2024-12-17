using AutoMapper;
using SIMAPI.Business.Enums;
using SIMAPI.Business.Helper;
using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models;
using SIMAPI.Repository.Interfaces;
using System.Net;

namespace SIMAPI.Business.Services
{
    public class NetworkService : INetworkService
    {
        private readonly INetworkRepository _networkRepository;
        private readonly IMapper _mapper;
        public NetworkService(INetworkRepository networkRepository, IMapper mapper)
        {
            _networkRepository = networkRepository;
            _mapper = mapper;
        }

        public async Task<CommonResponse> CreateAsync(NetworkDto request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var networkDBObject = await _networkRepository.GetNetworkByNameAsync(request.NetworkName);
                if (networkDBObject != null)
                {
                    response = Utility.CreateResponse("Network name already exist", HttpStatusCode.Conflict);
                }
                else
                {
                    networkDBObject = _mapper.Map<Network>(request);
                    networkDBObject.Status = (short)EnumStatus.Active;
                    _networkRepository.Add(networkDBObject);
                    await _networkRepository.SaveChangesAsync();
                    response = Utility.CreateResponse(request, HttpStatusCode.Created);
                }
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex);
            }
            return response;
        }

        public async Task<CommonResponse> UpdateAsync(NetworkDto request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var networkDBData = await _networkRepository.GetNetworkByNameAsync(request.NetworkName);
                if (networkDBData != null && networkDBData.NetworkId != request.NetworkId)
                {
                    response = Utility.CreateResponse("Network name already exist", HttpStatusCode.Conflict);
                }
                else
                {
                    networkDBData = await _networkRepository.GetNetworkByIdAsync(request.NetworkId);
                    networkDBData.NetworkName = request.NetworkName;
                    networkDBData.NetworkCode = request.NetworkCode;
                    networkDBData.SkuCode = request.SkuCode;
                    networkDBData.Status = request.Status;
                    await _networkRepository.SaveChangesAsync();
                    response = Utility.CreateResponse(networkDBData, HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex);
            }
            return response;
        }


        public async Task<CommonResponse> DeleteAsync(int id)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var networkDBData = await _networkRepository.GetNetworkByIdAsync(id);
                if (networkDBData != null)
                {
                    networkDBData.Status = (short)EnumStatus.Deleted;
                    await _networkRepository.SaveChangesAsync();
                    response = Utility.CreateResponse(networkDBData, HttpStatusCode.OK);
                }
                else
                {
                    response = Utility.CreateResponse("Network name does not exist", HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex);
            }
            return response;
        }

        public async Task<CommonResponse> GetByIdAsync(int id)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _networkRepository.GetNetworkByIdAsync(id);
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex);
            }
            return response;
        }

        public async Task<CommonResponse> GetByNameAsync(string name)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _networkRepository.GetNetworkByNameAsync(name);
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex);
            }
            return response;
        }

        // Get All Active Networks only
        public async Task<CommonResponse> GetAllAsync()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _networkRepository.GetAllNetworksAsync();
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex);
            }
            return response;
        }

        public async Task<CommonResponse> GetByPagingAsync(GetPagedSearch request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                PagedResult pageResult = new PagedResult();
                pageResult.Results = await _networkRepository.GetNetworksByPagingAsync(request);
                pageResult.TotalRecords = await _networkRepository.GetTotalNetworksCountAsync(request);
                response = Utility.CreateResponse(pageResult, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex);
            }
            return response;
        }
    }
}
