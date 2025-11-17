using AutoMapper;
using SIMAPI.Business.Helper;
using SIMAPI.Business.Interfaces;
using SIMAPI.Data;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models;
using SIMAPI.Repository.Interfaces;
using System.Net;


namespace SIMAPI.Business.Services
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseRepository _repo;
        private readonly IMapper _mapper;
        private readonly SIMDBContext _context;

        public PurchaseService(IPurchaseRepository repo, IMapper mapper, SIMDBContext context)
        {
            _repo = repo;
            _mapper = mapper;
            _context = context;
        }



        public async Task<CommonResponse> CreatePurchaseAsync(PurchaseInvoiceCreateDto request)
        {
            CommonResponse response = new CommonResponse();
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var invoiceDbo = _mapper.Map<PurchaseInvoice>(request);
                    _repo.Add(invoiceDbo);
                    await _repo.SaveChangesAsync();

                    foreach (var item in request.Items)
                    {
                        var itemDbo = _mapper.Map<PurchaseInvoiceItem>(item);
                        itemDbo.PurchaseInvoiceId = invoiceDbo.PurchaseInvoiceId;

                        _repo.Add(itemDbo);
                        await _repo.SaveChangesAsync();
                    }
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    response = response.HandleException(ex, _repo);
                }
            }
            return response;
        }

        public async Task<CommonResponse> UpdatePurchaseAsync(PurchaseInvoiceCreateDto request)
        {
            CommonResponse response = new CommonResponse();
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var invoiceDbo = _mapper.Map<PurchaseInvoice>(request);
                    _repo.Add(invoiceDbo);
                    await _repo.SaveChangesAsync();

                    foreach (var item in request.Items)
                    {
                        var itemDbo = _mapper.Map<PurchaseInvoiceItem>(item);
                        itemDbo.PurchaseInvoiceId = invoiceDbo.PurchaseInvoiceId;

                        _repo.Add(itemDbo);
                        await _repo.SaveChangesAsync();
                    }
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    response = response.HandleException(ex, _repo);
                }
            }
            return response;
        }


        public async Task<CommonResponse> GetByIdAsync(int id)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                var result = await _repo.GetPurchaseInvoiceDetailsByIdAsync(id);
                response = Utility.CreateResponse(result, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _repo);
            }
            return response;
        }       

      

        public async Task<CommonResponse> GetByPagingAsync(GetPagedSearch request)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                PagedResult pageResult = new PagedResult();
                pageResult.Results = await _repo.GetInvoiceListPagingAsync(request);
                pageResult.TotalRecords = await _repo.GetTotalInvoicesCountAsync(request);
                response = Utility.CreateResponse(pageResult, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                response = response.HandleException(ex, _repo);
            }
            return response;
        }
    }

}
