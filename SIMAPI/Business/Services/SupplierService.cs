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
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;
        private readonly IMapper _mapper;
        public SupplierService(ISupplierRepository SupplierRepository, IMapper mapper)
        {
            _supplierRepository = SupplierRepository;
            _mapper = mapper;
        }

        public async Task<CommonResponse> CreateAsync(SupplierDto request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var supplierDbo = await _supplierRepository.GetSupplierByNameAsync(request.SupplierName);
                if (supplierDbo != null)
                {
                    response = Utility.CreateResponse("Supplier is already exist", HttpStatusCode.Conflict);
                }
                else
                {
                    supplierDbo = new Supplier();
                    supplierDbo.SupplierName = request.SupplierName;
                    supplierDbo.Status = request.Status;
                    supplierDbo.CreatedBy = request.CreatedBy.Value;
                    supplierDbo.CreatedDate = DateTime.Now;
                    _supplierRepository.Add(supplierDbo);
                    await _supplierRepository.SaveChangesAsync();

                    if (request.SupplierAccounts.Any())
                    {
                        foreach (var account in request.SupplierAccounts)
                        {
                            var supplierAccount = new SupplierAccount();
                            supplierAccount.AccountName = account.AccountName;
                            supplierAccount.AccountNumber = account.AccountNumber;
                            supplierAccount.SupplierId = supplierDbo.SupplierId;
                            supplierAccount.Status = (short)EnumStatus.Active;
                            supplierAccount.CreatedBy = request.CreatedBy.Value;
                            supplierAccount.CreatedDate = DateTime.Now;
                            _supplierRepository.Add(supplierAccount);
                        }
                        await _supplierRepository.SaveChangesAsync();
                    }

                    if (request.SupplierProducts.Any())
                    {
                        foreach (var product in request.SupplierProducts)
                        {
                            var supplierProduct = new SupplierProduct();
                            supplierProduct.ProductId = product.ProductId;
                            supplierProduct.ProductCost = product.ProductCost;
                            supplierProduct.SupplierId = supplierDbo.SupplierId;
                            supplierProduct.Status = (short)EnumStatus.Active;
                            supplierProduct.CreatedBy = request.CreatedBy.Value;
                            supplierProduct.CreatedDate = DateTime.Now;
                            _supplierRepository.Add(supplierProduct);
                        }
                        await _supplierRepository.SaveChangesAsync();
                    }

                    response = Utility.CreateResponse(supplierDbo, HttpStatusCode.Created);
                }
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _supplierRepository);
            }
            return response;
        }

        public async Task<CommonResponse> UpdateAsync(SupplierDto request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var supplierDbo = await _supplierRepository.GetSupplierByNameAsync(request.SupplierName);
                if (supplierDbo != null && supplierDbo.SupplierId != request.SupplierId)
                {
                    response = Utility.CreateResponse("Supplier is already exist", HttpStatusCode.Conflict);
                }
                else
                {
                    var supplierDetails = await _supplierRepository.GetSupplierDetailsAsync(request.SupplierId);
                    supplierDbo = supplierDetails.supplier;
                    supplierDbo.SupplierName = request.SupplierName;
                    supplierDbo.Status = request.Status;
                    supplierDbo.UpdatedDate = DateTime.Now;
                    await _supplierRepository.SaveChangesAsync();

                    if (request.SupplierAccounts != null)
                    {
                        foreach (var savedAccount in supplierDetails.supplierAccounts)
                        {
                            var matchedAccount = request.SupplierAccounts.Where(w => w.SupplierAccountId == savedAccount.SupplierAccountId).FirstOrDefault();
                            if (matchedAccount != null)
                            {
                                savedAccount.AccountName = matchedAccount.AccountName;
                                savedAccount.AccountNumber = matchedAccount.AccountNumber;
                                savedAccount.UpdatedDate = DateTime.Now;
                            }
                            else
                            {
                                savedAccount.Status = (int)EnumStatus.Deleted;
                            }
                        }

                        foreach (var account in request.SupplierAccounts.Where(c => c.SupplierAccountId == 0))
                        {
                            var supplierAccount = new SupplierAccount();
                            supplierAccount.AccountName = account.AccountName;
                            supplierAccount.AccountNumber = account.AccountNumber;
                            supplierAccount.SupplierId = supplierDbo.SupplierId;
                            supplierAccount.Status = (short)EnumStatus.Active;
                            supplierAccount.CreatedBy = request.CreatedBy.Value;
                            supplierAccount.CreatedDate = DateTime.Now;
                            _supplierRepository.Add(supplierAccount);

                        }
                        await _supplierRepository.SaveChangesAsync();
                    }

                    if (request.SupplierProducts != null)
                    {
                        foreach (var savedProduct in supplierDetails.supplierProducts)
                        {
                            var matchedItem = request.SupplierProducts.Where(w => w.SupplierProductId == savedProduct.SupplierProductId).FirstOrDefault();
                            if (matchedItem != null)
                            {
                                savedProduct.ProductId = matchedItem.ProductId;
                                savedProduct.ProductCost = matchedItem.ProductCost;
                                savedProduct.UpdatedDate = DateTime.Now;
                            }
                            else
                            {
                                savedProduct.Status = (int)EnumStatus.Deleted;
                            }
                        }
                        foreach (var product in request.SupplierProducts.Where(c => c.SupplierProductId == 0))
                        {
                            var supplierProduct = new SupplierProduct();
                            supplierProduct.ProductId = product.ProductId;
                            supplierProduct.ProductCost = product.ProductCost;
                            supplierProduct.SupplierId = supplierDbo.SupplierId;
                            supplierProduct.Status = (short)EnumStatus.Active;
                            supplierProduct.CreatedBy = request.CreatedBy.Value;
                            supplierProduct.CreatedDate = DateTime.Now;
                            _supplierRepository.Add(supplierProduct);
                        }
                    }

                    await _supplierRepository.SaveChangesAsync();
                    response = Utility.CreateResponse(supplierDbo, HttpStatusCode.OK);
                }
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _supplierRepository);
            }
            return response;
        }

        public async Task<CommonResponse> DeleteAsync(int id)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var SupplierDBData = await _supplierRepository.GetSupplierByIdAsync(id);
                if (SupplierDBData != null)
                {
                    SupplierDBData.Status = (int)EnumStatus.Deleted;
                    SupplierDBData.UpdatedDate = DateTime.Now;
                    await _supplierRepository.SaveChangesAsync();
                    response = Utility.CreateResponse(SupplierDBData, HttpStatusCode.OK);
                }
                else
                {
                    response = Utility.CreateResponse("Supplier name does not exist", HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _supplierRepository);
            }
            return response;
        }

        public async Task<CommonResponse> GetByIdAsync(int id)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _supplierRepository.GetSupplierDetailsAsync(id);
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _supplierRepository);
            }
            return response;
        }

        public async Task<CommonResponse> GetByNameAsync(string name)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _supplierRepository.GetSupplierByNameAsync(name);
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _supplierRepository);
            }
            return response;
        }

        //Get all active Suppliers only
        public async Task<CommonResponse> GetAllAsync()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _supplierRepository.GetAllSuppliersAsync();
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _supplierRepository);
            }
            return response;
        }

        public async Task<CommonResponse> GetByPagingAsync(GetPagedSearch request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                PagedResult pageResult = new PagedResult();
                pageResult.Results = await _supplierRepository.GetSuppliersByPagingAsync(request);
                pageResult.TotalRecords = await _supplierRepository.GetTotalSuppliersCountAsync(request);
                response = Utility.CreateResponse(pageResult, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _supplierRepository);
            }
            return response;
        }





        public async Task<CommonResponse> GetSupplierReportAsync(GetReportRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<CommonResponse> GetHistoricalSupplierReportAsync(GetReportRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
