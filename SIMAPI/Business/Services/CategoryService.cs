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
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public CategoryService(ICategoryRepository CategoryRepository, IMapper mapper)
        {
            _categoryRepository = CategoryRepository;
            _mapper = mapper;
        }

        public async Task<CommonResponse> CreateAsync(CategoryDto request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var categoryDbo = await _categoryRepository.GetCategoryByNameAsync(request.CategoryName);
                if (categoryDbo != null)
                {
                    response = Utility.CreateResponse("Category is already exist", HttpStatusCode.Conflict);
                }
                else
                {
                    categoryDbo = _mapper.Map<Category>(request);
                    categoryDbo.Status = (short)EnumStatus.Active;
                    categoryDbo.CreatedDate = DateTime.Now;
                    if (request.ImageFile != null)
                    {
                        categoryDbo.Image = FileUtility.uploadImage(request.ImageFile, FolderUtility.category);
                    }
                    _categoryRepository.Add(categoryDbo);
                    await _categoryRepository.SaveChangesAsync();
                    response = Utility.CreateResponse(categoryDbo, HttpStatusCode.Created);
                }
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _categoryRepository);
            }
            return response;
        }

        public async Task<CommonResponse> UpdateAsync(CategoryDto request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var categoryDbo = await _categoryRepository.GetCategoryByNameAsync(request.CategoryName);
                if (categoryDbo != null && categoryDbo.CategoryId != request.CategoryId)
                {
                    response = Utility.CreateResponse("Category is already exist", HttpStatusCode.Conflict);
                }
                else
                {
                    categoryDbo = await _categoryRepository.GetCategoryByIdAsync(request.CategoryId);
                    categoryDbo.ModifiedDate = DateTime.Now;
                    categoryDbo.CategoryName = request.CategoryName;
                    categoryDbo.Status = request.Status;
                    if (request.ImageFile != null)
                    {
                        categoryDbo.Image = FileUtility.uploadImage(request.ImageFile, FolderUtility.category);
                    }
                    await _categoryRepository.SaveChangesAsync();
                    response = Utility.CreateResponse(categoryDbo, HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _categoryRepository);
            }
            return response;
        }

        public async Task<CommonResponse> DeleteAsync(int id)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var CategoryDBData = await _categoryRepository.GetCategoryByIdAsync(id);
                if (CategoryDBData != null)
                {
                    CategoryDBData.Status = (short)EnumStatus.Deleted;
                    CategoryDBData.ModifiedDate = DateTime.Now;
                    await _categoryRepository.SaveChangesAsync();
                    response = Utility.CreateResponse(CategoryDBData, HttpStatusCode.OK);
                }
                else
                {
                    response = Utility.CreateResponse("Category name does not exist", HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _categoryRepository);
            }
            return response;
        }

        public async Task<CommonResponse> GetByIdAsync(int id)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _categoryRepository.GetCategoryDetailsByIdAsync(id);
                if (!string.IsNullOrEmpty(result.Image))
                    result.Image = FileUtility.GetImagePath(FolderUtility.category, result.Image);
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _categoryRepository);
            }
            return response;
        }

        public async Task<CommonResponse> GetByNameAsync(string name)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _categoryRepository.GetCategoryByNameAsync(name);
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _categoryRepository);
            }
            return response;
        }

        //Get all active Categorys only
        public async Task<CommonResponse> GetAllAsync()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _categoryRepository.GetAllCategorysAsync();
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _categoryRepository);
            }
            return response;
        }

        public async Task<CommonResponse> GetByPagingAsync(GetPagedSearch request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                PagedResult pageResult = new PagedResult();
                pageResult.Results = await _categoryRepository.GetCategorysByPagingAsync(request);
                pageResult.TotalRecords = await _categoryRepository.GetTotalCategorysCountAsync(request);
                response = Utility.CreateResponse(pageResult, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _categoryRepository);
            }
            return response;
        }

    }
}
