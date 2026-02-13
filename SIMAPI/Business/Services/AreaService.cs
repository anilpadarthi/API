using AutoMapper;
using DocumentFormat.OpenXml.Presentation;
using SIMAPI.Business.Enums;
using SIMAPI.Business.Helper;
using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models;
using SIMAPI.Repository.Interfaces;
using SIMAPI.Repository.Repositories;
using System.Net;


namespace SIMAPI.Business.Services
{
    public class AreaService : IAreaService
    {
        private readonly IAreaRepository _areaRepository;
        private readonly IMapper _mapper;
        public AreaService(IAreaRepository areaRepository, IMapper mapper)
        {
            _areaRepository = areaRepository;
            _mapper = mapper;
        }

        public async Task<CommonResponse> CreateAsync(AreaDto request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var areaDbo = await _areaRepository.GetAreaByNameAsync(request.AreaName);
                if (areaDbo != null)
                {
                    response = Utility.CreateResponse("Area is already exist", HttpStatusCode.Conflict);
                }
                else
                {
                    areaDbo = _mapper.Map<Area>(request);
                    areaDbo.Status = (short)EnumStatus.Active;
                    areaDbo.CreatedDate = DateTime.Now;
                    areaDbo.OldAreaId = await _areaRepository.GetNextOldAreaIdAsync() + 1;
                    _areaRepository.Add(areaDbo);
                    await _areaRepository.SaveChangesAsync();
                    await CreateAreaLog(areaDbo);
                    AreaMap areaMap = new AreaMap();
                    areaMap.AreaId = areaDbo.AreaId;
                    areaMap.UserId = request.CreatedBy.Value;
                    areaMap.IsActive = true;
                    areaMap.MappedDate = DateTime.Now;
                    areaMap.FromDate = DateTime.Now;
                    await CreateAreaMap(areaMap);
                    response = Utility.CreateResponse(areaDbo, HttpStatusCode.Created);
                }
            }
            catch (Exception ex)
            {
                response = await response.HandleException(ex, _areaRepository);
            }
            return response;
        }

        public async Task<CommonResponse> UpdateAsync(AreaDto request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var areaDbo = await _areaRepository.GetAreaByNameAsync(request.AreaName);
                if (areaDbo != null && areaDbo.AreaId != request.AreaId)
                {
                    response = Utility.CreateResponse("Area is already exist", HttpStatusCode.Conflict);
                }
                else
                {
                    areaDbo = await _areaRepository.GetAreaByIdAsync(request.AreaId);
                    areaDbo.UpdatedDate = DateTime.Now;
                    areaDbo.AreaName = request.AreaName;
                    areaDbo.Status = request.Status;
                    await _areaRepository.SaveChangesAsync();
                    await CreateAreaLog(areaDbo);
                    response = Utility.CreateResponse(areaDbo, HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                response = await response.HandleException(ex, _areaRepository);
            }
            return response;
        }

        public async Task<CommonResponse> DeleteAsync(int id)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var areaDBData = await _areaRepository.GetAreaByIdAsync(id);
                if (areaDBData != null)
                {
                    areaDBData.Status = (short)EnumStatus.Deleted;
                    areaDBData.UpdatedDate = DateTime.Now;
                    await _areaRepository.SaveChangesAsync();
                    await CreateAreaLog(areaDBData);
                    response = Utility.CreateResponse(areaDBData, HttpStatusCode.OK);
                }
                else
                {
                    response = Utility.CreateResponse("Area name does not exist", HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                response = await response.HandleException(ex, _areaRepository);
            }
            return response;
        }

        public async Task<CommonResponse> ViewAreaAllocationHistorySync(int id)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _areaRepository.ViewAreaAllocationHistorySync(id);
                response = Utility.CreateResponse(result, HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                response = await response.HandleException(ex, _areaRepository);
            }
            return response;
        }
        public async Task<CommonResponse> GetByIdAsync(int id)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _areaRepository.GetAreaByIdAsync(id);
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = await response.HandleException(ex, _areaRepository);
            }
            return response;
        }

        public async Task<CommonResponse> GetByNameAsync(string name)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _areaRepository.GetAreaByNameAsync(name);
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = await response.HandleException(ex, _areaRepository);
            }
            return response;
        }

        //Get all active areas only
        public async Task<IEnumerable<Area>> GetAllAsync()
        {
            return await _areaRepository.GetAllAreasAsync();
        }

        public async Task<CommonResponse> GetByPagingAsync(GetPagedSearch request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                PagedResult pageResult = new PagedResult();
                pageResult.Results = await _areaRepository.GetAreasByPagingAsync(request);
                pageResult.TotalRecords = await _areaRepository.GetTotalAreasCountAsync(request);
                response = Utility.CreateResponse(pageResult, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = await response.HandleException(ex, _areaRepository);
            }
            return response;
        }

        public async Task<CommonResponse> CreateAreaMapAsync(int[] areaIds, int userId)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                foreach (var areaId in areaIds)
                {
                    var areaMapDBData = await _areaRepository.GetAreaMapByAreaIdAsync(areaId);
                    if (areaMapDBData != null)
                    {
                        if (areaMapDBData.UserId != userId)
                        {
                            areaMapDBData.IsActive = false;
                            areaMapDBData.ToDate = DateTime.Now;
                            areaMapDBData.MappedDate = DateTime.Now;
                            await _areaRepository.SaveChangesAsync();

                            AreaMap areaMap = new AreaMap();
                            areaMap.AreaId = areaId;
                            areaMap.UserId = userId;
                            areaMap.IsActive = true;
                            areaMap.MappedDate = DateTime.Now;
                            areaMap.FromDate = DateTime.Now;
                            _areaRepository.Add(areaMap);
                            await _areaRepository.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        AreaMap areaMap = new AreaMap();
                        areaMap.AreaId = areaId;
                        areaMap.UserId = userId;
                        areaMap.IsActive = true;
                        areaMap.MappedDate = DateTime.Now;
                        areaMap.FromDate = DateTime.Now;
                        _areaRepository.Add(areaMap);
                        await _areaRepository.SaveChangesAsync();
                    }

                }
                response = Utility.CreateResponse("Successfully Mapped.", HttpStatusCode.Created);
            }
            catch (Exception ex)
            {
                response = await response.HandleException(ex, _areaRepository);
            }
            return response;
        }

        public async Task<CommonResponse> AllocateAreasToUserAsync(AllocateAreaDto request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                foreach (var id in request.areaIds)
                {
                    var existingAreaMap = await _areaRepository.GetAreaMapByAreaIdAsync(id);
                    if (existingAreaMap != null)
                    {
                        existingAreaMap.IsActive = false;
                        existingAreaMap.ToDate = request.fromDate ??  DateTime.Now;
                        existingAreaMap.MappedDate = DateTime.Now;
                        await _areaRepository.SaveChangesAsync();
                    }

                    AreaMap amap = new AreaMap();
                    amap.AreaId = id;
                    amap.UserId = request.agentId;
                    amap.FromDate = request.fromDate ?? new DateTime();
                    amap.IsActive = true;
                    amap.MappedDate = DateTime.Now;
                    _areaRepository.Add(amap);
                    await _areaRepository.SaveChangesAsync();
                }

                response = Utility.CreateResponse("Allocated successfully", HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = await response.HandleException(ex, _areaRepository);
            }
            return response;
        }

        public async Task<CommonResponse> DeAllocateAreasToUserAsync(int[] areaIds, int userId)
        {
            throw new NotImplementedException();
        }

        public async Task<CommonResponse> GetAllAreasToAllocateAsync(int loggedInUserId, int userRoleId)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _areaRepository.GetAllAreasToAllocateAsync(loggedInUserId, userRoleId);

                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = await response.HandleException(ex, _areaRepository);
            }
            return response;
        }

        private async Task CreateAreaLog(Area area)
        {
            var log = _mapper.Map<AreaLog>(area);
            _areaRepository.Add(log);
            await _areaRepository.SaveChangesAsync();
        }

        private async Task CreateAreaMap(AreaMap areamap)
        {
            var log = _mapper.Map<AreaMap>(areamap);
            _areaRepository.Add(log);
            await _areaRepository.SaveChangesAsync();
        }
    }
}
