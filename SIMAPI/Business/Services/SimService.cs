using AutoMapper;
using SIMAPI.Business.Helper;
using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models;
using SIMAPI.Repository.Interfaces;
using System.Net;
using System.Text;

namespace SIMAPI.Business.Services
{
    public class SimService : ISimService
    {
        private readonly ISimRepository _simRepository;
        private readonly INetworkRepository _networkRepository;
        private readonly IMapper _mapper;
        public SimService(ISimRepository simRepository, INetworkRepository networkRepository, IMapper mapper)
        {
            _simRepository = simRepository;
            _networkRepository = networkRepository;
            _mapper = mapper;
        }

        public async Task<CommonResponse> AllocateSimsAsync(GetSimInfoRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                if (request.imeiNumbers?.Length > 0 && request.shopId.HasValue)
                {
                    int totalAllcated = 0;
                    foreach (var imei in request.imeiNumbers)
                    {
                        var simDetails = await _simRepository.GetSimDetailsAsync(imei);
                        if (simDetails != null)
                        {
                            var simMapDetails = await _simRepository.GetSimMapDetailsAsync(simDetails.SimId);
                            if (simMapDetails == null)
                            {
                                SimMap smap = new SimMap();
                                smap.SimId = simDetails.SimId;
                                smap.ShopId = request.shopId ?? 0;
                                smap.UserId = request.loggedInUserId ?? 0;
                                smap.MappedDate = DateTime.Now;
                                smap.CreatedDate = DateTime.Now;
                                smap.IsActive = true;
                                _simRepository.Add(smap);
                                await _simRepository.SaveChangesAsync();
                                totalAllcated++;
                                await SyncSimAPI(request.shopId ?? 0, simDetails.SimId, simDetails.NetworkId, request.loggedInUserId ?? 0);
                            }
                        }
                    }
                    await LogUserTrack(request);

                    response = Utility.CreateResponse("Total " + totalAllcated + " Sim cards are allocated", HttpStatusCode.OK);

                }
                else
                {
                    response = Utility.CreateResponse("IMEI or ShopId can not be empty", HttpStatusCode.OK);
                }

            }
            catch (Exception ex)
            {
                response = response.HandleException(ex);
            }
            return response;
        }

        public async Task<CommonResponse> DeAllocateSimsAsync(GetSimInfoRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                if (request.imeiNumbers.Length > 0 && request.shopId.HasValue)
                {
                    int totalDeAllcated = 0;
                    foreach (var imei in request.imeiNumbers)
                    {
                        var simDetails = await _simRepository.GetSimDetailsAsync(imei);
                        if (simDetails != null)
                        {
                            var simMapDetails = await _simRepository.GetSimMapDetailsAsync(simDetails.SimId);
                            if (simMapDetails != null)
                            {
                                SimMapChangeLog sChangeLog = new SimMapChangeLog();
                                sChangeLog.SimId = simMapDetails.SimId;
                                sChangeLog.ShopId = simMapDetails.ShopId;
                                sChangeLog.UserId = simMapDetails.UserId;
                                sChangeLog.MappedDate = simMapDetails.MappedDate;
                                sChangeLog.DeAllocatedBy = request.loggedInUserId.Value;
                                sChangeLog.CreatedDate = DateTime.Now;
                                _simRepository.Add(sChangeLog);
                                await _simRepository.SaveChangesAsync();

                                _simRepository.Remove(simMapDetails);
                                await _simRepository.SaveChangesAsync();
                                totalDeAllcated++;
                            }
                        }
                    }
                    response = Utility.CreateResponse("Total " + totalDeAllcated + " Sim cards are De-allocated", HttpStatusCode.OK);
                }
                else
                {
                    response = Utility.CreateResponse("IMEI or ShopId can not be empty", HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex);
            }
            return response;
        }

        public async Task<CommonResponse> GetSimHistoryDetailsAsync(GetSimInfoRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                if (request.imeiNumbers.Length > 0)
                {
                    int totalAllcated = 0;
                    StringBuilder simNumbersBuilder = new StringBuilder();
                    simNumbersBuilder.Append("<SimNumbers>");
                    foreach (var imei in request.imeiNumbers)
                    {
                        simNumbersBuilder.Append("<Sim>");
                        simNumbersBuilder.Append("<IMEI>");
                        simNumbersBuilder.Append(imei);
                        simNumbersBuilder.Append("</IMEI>");
                        simNumbersBuilder.Append("</Sim>");
                    }
                    simNumbersBuilder.Append("</SimNumbers>");
                    var result = await _simRepository.GetSimHistoryDetailsAsync(simNumbersBuilder);
                    response = Utility.CreateResponse(result, HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex);
            }
            return response;
        }

        public async Task<CommonResponse> ScanSimsAsync(GetSimInfoRequest request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                if (request.imeiNumbers.Length > 0 && request.shopId.HasValue)
                {
                    int totalAllcated = 0;
                    var simList = request.imeiNumbers;
                    StringBuilder simNumbersBuilder = new StringBuilder();
                    simNumbersBuilder.Append("<SimNumbers>");

                    foreach (var imei in request.imeiNumbers)
                    {
                        simNumbersBuilder.Append("<Sim>");
                        simNumbersBuilder.Append("<IMEI>");
                        simNumbersBuilder.Append(imei);
                        simNumbersBuilder.Append("</IMEI>");
                        simNumbersBuilder.Append("<PCNNO>");
                        simNumbersBuilder.Append(imei);
                        simNumbersBuilder.Append("</PCNNO>");
                        simNumbersBuilder.Append("<SimNetworkType></SimNetworkType>");
                        simNumbersBuilder.Append("</Sim>");
                    }
                    simNumbersBuilder.Append("</SimNumbers>");
                    if (request.moblieNumbers != null)
                    {
                        await UpdateLebaraMobileNumberAsync(request.moblieNumbers);
                    }
                    var result = await _simRepository.ScanSimsAsync(simNumbersBuilder);
                    response = Utility.CreateResponse(result, HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex);
            }
            return response;
        }

        private async Task LogUserTrack(GetSimInfoRequest request)
        {
            UserTrack userTrack = new UserTrack();
            userTrack.ShopId = request.shopId;
            userTrack.UserId = request.loggedInUserId ?? 0;
            userTrack.TrackedDate = DateTime.Now;
            userTrack.CreatedDate = DateTime.Now;
            userTrack.WorkType = "Filed";
            userTrack.Latitude = request.Latitude;
            userTrack.Longitude = request.Longitude;
            _simRepository.Add(userTrack);
            await _simRepository.SaveChangesAsync();
        }

        private async Task SyncSimAPI(int shopId, int simId, int networkId, int loggedInUserId)
        {
            SimAPI obj = new SimAPI();
            obj.ShopId = shopId;
            obj.SimId = simId;
            obj.AssignedToShopByUserId = loggedInUserId;
            obj.NetworkId = networkId;
            var networkDetails = await _networkRepository.GetNetworkByIdAsync(networkId);
            var baseNetworkDetails = await _networkRepository.GetBaseNetworkByIdAsync(networkDetails.BaseNetworkId ?? 0);
            obj.NetworkName = networkDetails.NetworkName;
            obj.BaseNetwork = baseNetworkDetails.BaseNetworkName;
            obj.AssignedDate = DateTime.Now;
            _simRepository.Add(obj);
            try
            {
                await _simRepository.SaveChangesAsync();
            }
            catch (Exception ex)
            {

            }
        }

        public async Task UpdateLebaraMobileNumberAsync(string[] simList)
        {
            for (int i = 0; i < simList.Length; i++)
            {
                var simDetails = await _simRepository.GetSimDetailsAsync(simList[i]);
                if (simDetails != null)
                {
                    simDetails.PCNNO = simList[i + 1];
                }
                i = i + 1;
            }
            await _simRepository.SaveChangesAsync();


            //    var sqlParameters = new[]
            //    {
            //        new SqlParameter("@simNumbers", simNumbersBuilder.ToString())
            //    };
            //    await ExecuteStoredProcedureAsync("exec [dbo].[UpdateLebaraMobileNumber] @simNumbers", sqlParameters);
        }
    }
}
