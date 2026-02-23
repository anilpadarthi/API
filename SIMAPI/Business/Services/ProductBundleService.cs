using SIMAPI.Business.Helper;
using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models;
using SIMAPI.Repository.Interfaces;
using System.Net;


namespace SIMAPI.Business.Services
{
    public class ProductBundleService : IProductBundleService
    {
        private readonly IProductBundleRepository _productBundleRepository;

        public ProductBundleService(IProductBundleRepository productBundleRepository)
        {
            _productBundleRepository = productBundleRepository;
        }
        public async Task<CommonResponse> CreateAsync(ProductBundle request)
        {
            CommonResponse response = new CommonResponse();
            
                await _productBundleRepository.CreateAsync(request);
                response = Utility.CreateResponse("Created successfully", HttpStatusCode.Created);
           
            return response;
        }
        public async Task<CommonResponse> UpdateAsync(ProductBundle request)
        {
            CommonResponse response = new CommonResponse();
          
                await _productBundleRepository.UpdateAsync(request);
                response = Utility.CreateResponse("Updated successfully", HttpStatusCode.OK);
           
            return response;
        }
        public async Task<CommonResponse> DeleteAsync(int categoryId)
        {
            CommonResponse response = new CommonResponse();
           
                await _productBundleRepository.DeleteAsync(categoryId);
                response = Utility.CreateResponse("Deleted successfully", HttpStatusCode.OK);
          
            return response;
        }
        public async Task<CommonResponse> GetAllAsync()
        {
            CommonResponse response = new CommonResponse();
           
                var result = await _productBundleRepository.GetAllAsync();
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
           
            return response;
        }
        public async Task<CommonResponse> GetByIdAsync(int categoryId)
        {
            CommonResponse response = new CommonResponse();
          
                var result = await _productBundleRepository.GetByIdAsync(categoryId);
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
          
            return response;
        }
        public async Task<CommonResponse> GetByPagingAsync(GetPagedSearch request)
        {
            CommonResponse response = new CommonResponse();
           
                var result = await _productBundleRepository.GetByPagingAsync(request);
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
           
            return response;
        }
    }
}
