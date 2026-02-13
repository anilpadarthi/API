using AutoMapper;
using DocumentFormat.OpenXml.InkML;
using SIMAPI.Business.Enums;
using SIMAPI.Business.Helper;
using SIMAPI.Business.Interfaces;
using SIMAPI.Data;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models;
using SIMAPI.Repository.Interfaces;
using SIMAPI.Repository.Repositories;
using System.Net;

namespace SIMAPI.Business.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly IMapper _mapper;
        private readonly SIMDBContext _context;

        public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository, ISubCategoryRepository subCategoryRepository, IMapper mapper, SIMDBContext context)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _subCategoryRepository = subCategoryRepository;
            _mapper = mapper;
            _context = context;
        }
        public async Task<CommonResponse> CreateAsync(ProductDto request)
        {
            CommonResponse response = new CommonResponse();
            int productId = 0;
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var product = await _productRepository.GetByNameAsync(request.ProductName);
                    if (product != null)
                    {
                        response = Utility.CreateResponse("Prodct name already exist", HttpStatusCode.Conflict);
                    }
                    else
                    {
                        product = _mapper.Map<Product>(request);
                        product.Status = (short)EnumStatus.Active;
                        product.CreatedDate = DateTime.Now;
                        product.CreatedBy = request.loggedInUserId;

                        if (request.ProductImageFile != null)
                        {
                            product.ProductImage = FileUtility.uploadImage(request.ProductImageFile, FolderUtility.product);
                        }
                        _productRepository.Add(product);
                        await _productRepository.SaveChangesAsync();
                        if (request.ProductPrices != null && request.ProductPrices.Any())
                        {
                            await UpdateOrCreateProductPrices(null, request.ProductPrices, product.ProductId);
                        }
                        if (request.BundleItems != null &&  request.BundleItems.Any())
                        {
                            await UpdateOrCreateBundleItems(null, request.BundleItems, product.ProductId);
                        }
                        //not required now
                        //await AddProductCommission(product.ProductId, request.CommissionToAgent.Value, request.CommissionToManager.Value);
                        response = Utility.CreateResponse(product, HttpStatusCode.Created);
                        await transaction.CommitAsync();
                    }
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    response = await response.HandleException(ex, _productRepository);
                }
            }
            return response;
        }

        public async Task<CommonResponse> AddProductImageAsync(ProductImageModel request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var product = await _productRepository.GetByIdAsync(request.ProductId);
                if (product == null)
                {
                    response = Utility.CreateResponse("Prodct does not  exist", HttpStatusCode.NotFound);
                }
                else
                {
                    if (request.ImageFile != null)
                    {
                        ProductImage productImageMap = new ProductImage();
                        productImageMap.ProductId = request.ProductId;
                        productImageMap.Image = FileUtility.uploadImage(request.ImageFile, FolderUtility.product);
                        _productRepository.Add(productImageMap);
                    }
                    response = Utility.CreateResponse("Product created successfully", HttpStatusCode.Created);
                    await _productRepository.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                response = await response.HandleException(ex, _productRepository);
            }
            return response;
        }

        public async Task<CommonResponse> UpdateAsync(ProductDto request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var product = await _productRepository.GetByNameAsync(request.ProductName);
                if (product != null && product.ProductId != request.ProductId)
                {
                    response = Utility.CreateResponse("Product name already exist", HttpStatusCode.Conflict);
                }
                else
                {
                    product = await _productRepository.GetByIdAsync(request.ProductId);
                    product.ProductName = request.ProductName;
                    product.ProductCode = request.ProductCode;
                    product.CategoryId = request.CategoryId;
                    product.SubCategoryId = request.SubCategoryId;
                    product.Description = request.Description;
                    product.Specification = request.Specification;
                    product.DisplayOrder = request.DisplayOrder;
                    product.BuyingPrice = request.BuyingPrice;
                    product.MixMatchGroupId = request.MixMatchGroupId;
                    product.Status = request.Status.Value;
                    product.IsOutOfStock = request.IsOutOfStock.Value;
                    product.IsNewArrival = request.IsNewArrival.Value;
                    product.IsBundle = request.IsBundle.Value;
                    product.SellingPrice = request.SellingPrice;
                    product.CommissionToAgent = request.CommissionToAgent;
                    product.CommissionToManager = request.CommissionToManager;
                    product.ModifiedDate = DateTime.Now;
                    product.ModifiedBy = request.loggedInUserId;

                    if (request.ProductImageFile != null)
                    {
                        product.ProductImage = FileUtility.uploadImage(request.ProductImageFile, FolderUtility.product);
                    }
                    var savedProductPrices = await _productRepository.GetProductPricesAsync(product.ProductId);
                    var savedBundleItems = await _productRepository.GetProductBundleByIdAsync(product.ProductId);
                    await UpdateOrCreateProductPrices(savedProductPrices, request.ProductPrices, product.ProductId);
                    await UpdateOrCreateBundleItems(savedBundleItems, request.BundleItems, product.ProductId);
                    //not required now
                    //await UpdateProductCommission(product, request.CommissionToAgent.Value, request.CommissionToManager.Value); 
                    response = Utility.CreateResponse(product, HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                response = await response.HandleException(ex, _productRepository);
            }
            return response;
        }

        public async Task<CommonResponse> UpdateStatusAsync(int id, bool status)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                await _productRepository.UpdateStatusAsync(id, status);
                response = Utility.CreateResponse("Updated successfully", HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = await response.HandleException(ex, _productRepository);
            }
            return response;
        }

        public async Task<CommonResponse> UpdateDisplayOrderAsync(int id, int displayOrder)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                await _productRepository.UpdateDisplayOrderAsync(id, displayOrder);
                response = Utility.CreateResponse("Updated successfully", HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = await response.HandleException(ex, _productRepository);
            }
            return response;
        }

        public async Task<CommonResponse> DeleteProductAsync(int id)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var userDBData = await _productRepository.GetByIdAsync(id);
                if (userDBData != null)
                {
                    userDBData.Status = (int)EnumStatus.Deleted;
                    userDBData.ModifiedDate = DateTime.Now;
                    await _productRepository.SaveChangesAsync();
                    response = Utility.CreateResponse(userDBData, HttpStatusCode.OK);
                }
                else
                {
                    response = Utility.CreateResponse("User name does not exist", HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                response = await response.HandleException(ex, _productRepository);
            }
            return response;
        }


        public async Task<CommonResponse> GetAllAsync()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _productRepository.GetAllAsync();
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = await response.HandleException(ex, _productRepository);
            }
            return response;
        }

        public async Task<CommonResponse> GetByIdAsync(int id)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _productRepository.GetProductDetailsAsync(id);
                if (!string.IsNullOrEmpty(result.product.ProductImage))
                    result.product.ProductImage = FileUtility.GetImagePath(FolderUtility.product, result.product.ProductImage);
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = await response.HandleException(ex, _productRepository);
            }
            return response;
        }

        public async Task<CommonResponse> GetByPagingAsync(GetPagedSearch request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                PagedResult pageResult = new PagedResult();
                pageResult.Results = await _productRepository.GetByPagingAsync(request);
                pageResult.TotalRecords = await _productRepository.GetTotalProductsCountAsync(request);
                response = Utility.CreateResponse(pageResult, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = await response.HandleException(ex, _productRepository);
            }
            return response;
        }

        public async Task<CommonResponse> GetAllProductsAsync(ProductSearchModel request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                PagedResult pageResult = new PagedResult();
                var productList = await _productRepository.GetAllProductsAsync(request);

                if (productList.Any())
                {
                    productList.ToList().ForEach(e => e.Image = FileUtility.GetImagePath(FolderUtility.product, e.Image));
                    pageResult.TotalRecords = ((ProductListModel)productList.ToList().FirstOrDefault()).TotalCount ?? 0;
                }
                pageResult.Results = productList;
                response = Utility.CreateResponse(pageResult, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = await response.HandleException(ex, _productRepository);
            }
            return response;
        }

        //public async Task<CommonResponse> CreateBundleProductAsync(BundleProductRequestModel request)
        //{
        //    CommonResponse response = new CommonResponse();
        //    int productId = 0;
        //    try
        //    {
        //        var product = await _productRepository.GetByNameAsync(request.ProductName);
        //        if (product != null)
        //        {
        //            response = Utility.CreateResponse("Prodct name already exist", HttpStatusCode.Conflict);
        //        }
        //        else
        //        {
        //            product = new Product();
        //            product.ProductName = request.ProductName;
        //            product.ProductCode = request.ProductCode;
        //            product.Description = request.Description;
        //            product.Specification = request.Specification;
        //            product.CategoryId = request.CategoryId;
        //            product.SubCategoryId = request.SubCategoryId;
        //            _productRepository.Add(product);
        //            await _productRepository.SaveChangesAsync();

        //            productId = product.ProductId;
        //            if (request.ImageFile != null)
        //            {
        //                ProductImage productImageMap = new ProductImage();
        //                productImageMap.ProductId = productId;
        //                productImageMap.Image = FileUtility.uploadImage(request.ImageFile, FolderUtility.product);
        //                _productRepository.Add(productImageMap);
        //            }
        //            if (request.SalePrice != null)
        //            {
        //                ProductPrice priceModel = new ProductPrice();
        //                priceModel.ProductId = productId;
        //                priceModel.SalePrice = Convert.ToDecimal(request.SalePrice);
        //                priceModel.FromQty = 0;
        //                priceModel.ToQty = 1000;
        //                _productRepository.Add(priceModel);
        //            }

        //            if (request.BundleProducts != null)
        //            {
        //                foreach (var item in request.BundleProducts)
        //                {
        //                    ProductBundle ProductBundle = new ProductBundle();
        //                    ProductBundle.ParentId = productId;
        //                    ProductBundle.ProductId = item.ProductId;
        //                    ProductBundle.Quantity = item.Quantity;
        //                    _productRepository.Add(ProductBundle);
        //                }
        //            }

        //            response = Utility.CreateResponse(product, HttpStatusCode.Created);
        //            await _productRepository.SaveChangesAsync();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        response = await response.HandleException(ex, _productRepository);
        //    }
        //    return response;
        //}

        //public async Task<CommonResponse> UpdateBundleProductAsync(BundleProductRequestModel request)
        //{
        //    CommonResponse response = new CommonResponse();
        //    try
        //    {
        //        var product = await _productRepository.GetByNameAsync(request.ProductName);
        //        if (product != null && product.ProductId != request.ProductId)
        //        {
        //            response = Utility.CreateResponse("Product name already exist", HttpStatusCode.Conflict);
        //        }
        //        else
        //        {
        //            product = await _productRepository.GetByIdAsync(request.ProductId);
        //            product.ProductName = request.ProductName;
        //            product.ProductCode = request.ProductCode;
        //            product.Description = request.Description;
        //            product.Specification = request.Specification;
        //            product.CategoryId = request.CategoryId;
        //            product.SubCategoryId = request.SubCategoryId;

        //            if (request.ImageFile != null)
        //            {
        //                ProductImage productImageMap = new ProductImage();
        //                productImageMap.ProductId = request.ProductId;
        //                productImageMap.Image = FileUtility.uploadImage(request.ImageFile, FolderUtility.product);
        //                _productRepository.Add(productImageMap);
        //            }

        //            await _productRepository.SaveChangesAsync();


        //            await _productRepository.SaveChangesAsync();
        //            response = Utility.CreateResponse(product, HttpStatusCode.Created);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        response = await response.HandleException(ex, _productRepository);
        //    }
        //    return response;
        //}

        private async Task UpdateOrCreateProductPrices(IEnumerable<ProductPrice>? savedProductPrices, List<ProductPriceDto> productPriceList, int productId)
        {
            if (savedProductPrices != null)
            {
                foreach (var savedDoc in savedProductPrices)
                {
                    var matchedDocument = productPriceList.Where(w => w.ProductPriceId == savedDoc.ProductPriceId).FirstOrDefault();
                    if (matchedDocument != null)
                    {
                        _mapper.Map(matchedDocument, savedDoc);
                    }
                    else
                    {
                        // Mark saved document as inactive or deleted
                        savedDoc.Status = (int)EnumStatus.Deleted; // Assuming 0 indicates inactive/deleted
                    }
                }
            }

            if (productPriceList != null)
            {
                // Process incoming contacts that are new (not found in saved contacts)
                foreach (var item in productPriceList?.Where(c => c.ProductPriceId == 0))
                {
                    var newDocument = _mapper.Map<ProductPrice>(item);
                    newDocument.ProductId = productId;
                    newDocument.Status = (int)EnumStatus.Active;
                    newDocument.CreatedDate = DateTime.Now;
                    newDocument.ModifiedDate = DateTime.Now;
                    _productRepository.Add(newDocument);
                }
            }

            await _productRepository.SaveChangesAsync();
        }

        private async Task UpdateOrCreateBundleItems(IEnumerable<ProductBundle>? savedBundleItems, List<ProductBundle> bundleItems, int productId)
        {
            // If caller didn't supply any bundle items, do not modify existing saved items.
            // This prevents marking all existing records inactive when the client omitted bundle items from the request.
            if (bundleItems == null)
            {
                return;
            }

            if (savedBundleItems != null)
            {
                // Precompute incoming ids to avoid repeated scanning and to correctly detect which saved records exist in the incoming list.
                var incomingIds = new HashSet<int>(bundleItems
                    .Where(b => b.ProductBundleId.HasValue && b.ProductBundleId.Value != 0)
                    .Select(b => b.ProductBundleId!.Value));

                foreach (var savedDoc in savedBundleItems)
                {
                    var savedId = savedDoc.ProductBundleId ?? 0;
                    if (incomingIds.Contains(savedId))
                    {
                        // Update existing record from incoming data
                        var incoming = bundleItems.First(b => (b.ProductBundleId ?? 0) == savedId);
                        _mapper.Map(incoming, savedDoc);
                        savedDoc.IsActive = true;
                    }
                    else
                    {
                        // Only mark inactive when the incoming list was provided but does not include this saved item.
                        savedDoc.IsActive = false;
                    }
                }
            }

            // Add new incoming items (those without a ProductBundleId or with 0)
            foreach (var item in bundleItems.Where(c => !c.ProductBundleId.HasValue || c.ProductBundleId == 0))
            {
                var newItem = new ProductBundle
                {
                    // Set parent link explicitly to the productId (BundleProductId appears to be the parent id in repo queries)
                    BundleProductId = productId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    DisplayOrder = item.DisplayOrder,
                    Price = item.Price,
                    IsActive = true
                };
                _productRepository.Add(newItem);
            }

            await _productRepository.SaveChangesAsync();
        }

        private async Task AddProductCommission(int productId, decimal commissionToAgent, decimal commissionToManager)
        {
            ProductCommission productCommission = new ProductCommission()
            {
                ProductId = productId,
                FromDate = DateTime.Now,
                IsActive = 1,
                CommissionToAgent = commissionToAgent,
                CommissionToManager = commissionToManager
            };
            _productRepository.Add(productCommission);
            await _productRepository.SaveChangesAsync();
        }

        private async Task UpdateProductCommission(Product product, decimal commissionToAgent, decimal commissionToManager)
        {
            var productCommission = await _productRepository.GetProductCommissionByIdAsync(product.ProductId);
            if (productCommission != null)
            {
                if (productCommission.CommissionToAgent != commissionToAgent || productCommission.CommissionToManager != commissionToManager)
                {
                    productCommission.IsActive = 0;
                    productCommission.ToDate = DateTime.Now;

                    ProductCommission newProductCommission = new ProductCommission()
                    {
                        ProductId = productCommission.ProductId,
                        FromDate = DateTime.Now,
                        IsActive = 1,
                        CommissionToAgent = commissionToAgent,
                        CommissionToManager = commissionToManager
                    };
                    _categoryRepository.Add(newProductCommission);
                    await _categoryRepository.SaveChangesAsync();
                }
            }
            else
            {
                await AddProductCommission(product.ProductId, commissionToAgent, commissionToManager);
            }
        }
    }
}
