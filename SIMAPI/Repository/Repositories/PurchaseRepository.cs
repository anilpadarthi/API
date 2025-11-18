using Microsoft.EntityFrameworkCore;
using SIMAPI.Data;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Repository.Interfaces;

namespace SIMAPI.Repository.Repositories
{
    public class PurchaseRepository : Repository, IPurchaseRepository
    {
        public PurchaseRepository(SIMDBContext context) : base(context)
        {
        }



        public async Task<PurchaseInvoice> GetPurchaseInvoiceDetailsByIdAsync(int purchaseInvoiceId)
        {
            var purchaseInvoiceDetails = await _context.Set<PurchaseInvoice>()
                 .Where(w => w.PurchaseInvoiceId == purchaseInvoiceId)
                 .FirstOrDefaultAsync();

            return purchaseInvoiceDetails;
        }

        public async Task<PurchaseInvoice> GetPurchaseInvoiceDetailsByNumberIdAsync(string invoiceNumber)
        {
            var purchaseInvoiceDetails = await _context.Set<PurchaseInvoice>()
                .Where(w => w.InvoiceNumber == invoiceNumber)
                .FirstOrDefaultAsync();

            return purchaseInvoiceDetails;
        }

        public async Task<IEnumerable<PurchaseInvoiceItem>> GetPurchaseInvoiceItemsByIdAsync(int purchaseInvoiceId)
        {
            return await _context.Set<PurchaseInvoiceItem>()
               .Where(w => w.PurchaseInvoiceId == purchaseInvoiceId)
               .ToListAsync();

        }


        public async Task<IEnumerable<PurchaseInvoice>> GetInvoiceListPagingAsync(GetPagedSearch request)
        {
            var query = _context.Set<PurchaseInvoice>().AsQueryable();

            if (!string.IsNullOrEmpty(request.searchText))
            {
                query = query.Where(w => w.InvoiceNumber.Contains(request.searchText));
            }
            if (request.id.HasValue)
            {
                query = query.Where(w => w.SupplierId == request.id.Value);
            }
            var result = await query
                .OrderBy(o => o.CreatedDate)
                .Skip((request.pageNo - 1) * request.pageSize)
                .Take(request.pageSize)
                .ToListAsync();

            return result;
        }

        public async Task<int> GetTotalInvoicesCountAsync(GetPagedSearch request)
        {
            var query = _context.Set<PurchaseInvoice>().AsQueryable();

            if (!string.IsNullOrEmpty(request.searchText))
            {
                query = query.Where(w => w.InvoiceNumber.Contains(request.searchText));
            }
            if (request.id.HasValue)
            {
                query = query.Where(w => w.SupplierId == request.id.Value);
            }
            return await query.CountAsync();
        }
    }
}
