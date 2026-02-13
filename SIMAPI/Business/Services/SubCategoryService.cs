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
    public class SubCategoryService : ISubCategoryService
    {
        private readonly ISubCategoryRepository _SubCategoryRepository;
        private readonly IMapper _mapper;
        public SubCategoryService(ISubCategoryRepository SubCategoryRepository, IMapper mapper)
        {
            _SubCategoryRepository = SubCategoryRepository;
            _mapper = mapper;
        }

        public async Task<CommonResponse> CreateAsync(SubCategoryDto request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var subCategoryDbo = await _SubCategoryRepository.GetSubCategoryByNameAsync(request.SubCategoryName);
                if (subCategoryDbo != null)
                {
                    response = Utility.CreateResponse("SubCategory is already exist", HttpStatusCode.Conflict);
                }
                else
                {
                    subCategoryDbo = _mapper.Map<SubCategory>(request);
                    subCategoryDbo.Status = (short)EnumStatus.Active;
                    subCategoryDbo.CreatedDate = DateTime.Now;
                    if (request.ImageFile != null)
                    {
                        subCategoryDbo.Image = FileUtility.uploadImage(request.ImageFile, FolderUtility.subCategory);
                    }
                    _SubCategoryRepository.Add(subCategoryDbo);
                    await _SubCategoryRepository.SaveChangesAsync();
                    response = Utility.CreateResponse(subCategoryDbo, HttpStatusCode.Created);
                }
            }
            catch (Exception ex)
            {
                response = await response.HandleException(ex, _SubCategoryRepository);
            }
            return response;
        }

        public async Task<CommonResponse> UpdateAsync(SubCategoryDto request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var subCategoryDbo = await _SubCategoryRepository.GetSubCategoryByNameAsync(request.SubCategoryName);
                if (subCategoryDbo != null && subCategoryDbo.SubCategoryId != request.SubCategoryId)
                {
                    response = Utility.CreateResponse("SubCategory is already exist", HttpStatusCode.Conflict);
                }
                else
                {
                    subCategoryDbo = await _SubCategoryRepository.GetSubCategoryByIdAsync(request.SubCategoryId);
                    subCategoryDbo.ModifiedDate = DateTime.Now;
                    subCategoryDbo.SubCategoryName = request.SubCategoryName;
                    subCategoryDbo.Status = request.Status;
                    if (request.ImageFile != null)
                    {
                        subCategoryDbo.Image = FileUtility.uploadImage(request.ImageFile, FolderUtility.subCategory);
                    }
                    await _SubCategoryRepository.SaveChangesAsync();
                    response = Utility.CreateResponse(subCategoryDbo, HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                response = await response.HandleException(ex, _SubCategoryRepository);
            }
            return response;
        }

        public async Task<CommonResponse> DeleteAsync(int id)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var SubCategoryDBData = await _SubCategoryRepository.GetSubCategoryByIdAsync(id);
                if (SubCategoryDBData != null)
                {
                    SubCategoryDBData.Status = (short)EnumStatus.Deleted;
                    SubCategoryDBData.ModifiedDate = DateTime.Now;
                    await _SubCategoryRepository.SaveChangesAsync();
                    response = Utility.CreateResponse(SubCategoryDBData, HttpStatusCode.OK);
                }
                else
                {
                    response = Utility.CreateResponse("SubCategory name does not exist", HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                response = await response.HandleException(ex, _SubCategoryRepository);
            }
            return response;
        }

        public async Task<CommonResponse> GetByIdAsync(int id)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _SubCategoryRepository.GetSubCategoryByIdAsync(id);
                if (!string.IsNullOrEmpty(result.Image))
                    result.Image = FileUtility.GetImagePath(FolderUtility.subCategory, result.Image);
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = await response.HandleException(ex, _SubCategoryRepository);
            }
            return response;
        }

        public async Task<CommonResponse> GetByNameAsync(string name)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _SubCategoryRepository.GetSubCategoryByNameAsync(name);
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = await response.HandleException(ex, _SubCategoryRepository);
            }
            return response;
        }

        //Get all active SubCategorys only
        public async Task<CommonResponse> GetAllAsync()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _SubCategoryRepository.GetAllSubCategorysAsync();
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = await response.HandleException(ex, _SubCategoryRepository);
            }
            return response;
        }

        public async Task<CommonResponse> GetByPagingAsync(GetPagedSearch request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                PagedResult pageResult = new PagedResult();
                pageResult.Results = await _SubCategoryRepository.GetSubCategorysByPagingAsync(request);
                pageResult.TotalRecords = await _SubCategoryRepository.GetTotalSubCategorysCountAsync(request);
                response = Utility.CreateResponse(pageResult, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = await response.HandleException(ex, _SubCategoryRepository);
            }
            return response;
        }
       
    }
}
