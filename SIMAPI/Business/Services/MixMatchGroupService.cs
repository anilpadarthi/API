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
    public class MixMatchGroupService : IMixMatchGroupService
    {
        private readonly IMixMatchGroupRepository _MixMatchGroupRepository;
        private readonly IMapper _mapper;
        public MixMatchGroupService(IMixMatchGroupRepository MixMatchGroupRepository, IMapper mapper)
        {
            _MixMatchGroupRepository = MixMatchGroupRepository;
            _mapper = mapper;
        }

        public async Task<CommonResponse> CreateAsync(MixMatchGroup request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var mixMatchGroupDbo = await _MixMatchGroupRepository.GetMixMatchGroupByNameAsync(request.GroupName);
                if (mixMatchGroupDbo != null)
                {
                    response = Utility.CreateResponse("MixMatchGroup is already exist", HttpStatusCode.Conflict);
                }
                else
                {
                    mixMatchGroupDbo = new MixMatchGroup();
                    mixMatchGroupDbo.GroupName = request.GroupName;
                    mixMatchGroupDbo.Status = (short)EnumStatus.Active;
                    mixMatchGroupDbo.CreatedDate = DateTime.Now;
                    _MixMatchGroupRepository.Add(mixMatchGroupDbo);
                    await _MixMatchGroupRepository.SaveChangesAsync();
                    response = Utility.CreateResponse(mixMatchGroupDbo, HttpStatusCode.Created);
                }
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _MixMatchGroupRepository);
            }
            return response;
        }

        public async Task<CommonResponse> UpdateAsync(MixMatchGroup request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var MixMatchGroupDbo = await _MixMatchGroupRepository.GetMixMatchGroupByNameAsync(request.GroupName);
                if (MixMatchGroupDbo != null && MixMatchGroupDbo.MixMatchGroupId != request.MixMatchGroupId)
                {
                    response = Utility.CreateResponse("MixMatchGroup is already exist", HttpStatusCode.Conflict);
                }
                else
                {
                    MixMatchGroupDbo = await _MixMatchGroupRepository.GetMixMatchGroupByIdAsync(request.MixMatchGroupId.Value);
                    MixMatchGroupDbo.GroupName = request.GroupName;
                    MixMatchGroupDbo.Status = request.Status;
                    await _MixMatchGroupRepository.SaveChangesAsync();
                    response = Utility.CreateResponse(MixMatchGroupDbo, HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _MixMatchGroupRepository);
            }
            return response;
        }

        public async Task<CommonResponse> DeleteAsync(int id)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var MixMatchGroupDBData = await _MixMatchGroupRepository.GetMixMatchGroupByIdAsync(id);
                if (MixMatchGroupDBData != null)
                {
                    MixMatchGroupDBData.Status = (short)EnumStatus.Deleted;
                    await _MixMatchGroupRepository.SaveChangesAsync();
                    response = Utility.CreateResponse(MixMatchGroupDBData, HttpStatusCode.OK);
                }
                else
                {
                    response = Utility.CreateResponse("MixMatchGroup name does not exist", HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _MixMatchGroupRepository);
            }
            return response;
        }

        public async Task<CommonResponse> GetByIdAsync(int id)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _MixMatchGroupRepository.GetMixMatchGroupByIdAsync(id);               
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _MixMatchGroupRepository);
            }
            return response;
        }

        public async Task<CommonResponse> GetByNameAsync(string name)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _MixMatchGroupRepository.GetMixMatchGroupByNameAsync(name);
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _MixMatchGroupRepository);
            }
            return response;
        }



        public async Task<CommonResponse> GetByPagingAsync(GetPagedSearch request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                PagedResult pageResult = new PagedResult();
                pageResult.Results = await _MixMatchGroupRepository.GetMixMatchGroupsByPagingAsync(request);
                pageResult.TotalRecords = await _MixMatchGroupRepository.GetTotalMixMatchGroupsCountAsync(request);
                response = Utility.CreateResponse(pageResult, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _MixMatchGroupRepository);
            }
            return response;
        }

    }
}
