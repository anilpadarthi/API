using Microsoft.EntityFrameworkCore;
using SIMAPI.Business.Enums;
using SIMAPI.Data;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models;
using SIMAPI.Repository.Interfaces;

namespace SIMAPI.Repository.Repositories
{
    public class SupplierRepository : Repository, ISupplierRepository
    {
        public SupplierRepository(SIMDBContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Supplier>> GetAllSuppliersAsync()
        {
            return await _context.Set<Supplier>()
                .Where(w => w.Status == (int)EnumStatus.Active)
                .ToListAsync();
        }

        public async Task<Supplier> GetSupplierByIdAsync(int supplierId)
        {
            return await _context.Set<Supplier>()
                .Where(w => w.SupplierId == supplierId && w.Status == (int)EnumStatus.Active)
                .FirstOrDefaultAsync();
        }

        public async Task<SupplierDetails> GetSupplierDetailsAsync(int supplierId)
        {
            SupplierDetails supplierDetails = new SupplierDetails();
            supplierDetails.supplier = await _context.Set<Supplier>()
                .Where(w => w.SupplierId == supplierId && w.Status == (int)EnumStatus.Active)
                .FirstOrDefaultAsync();
            supplierDetails.supplierAccounts = await _context.Set<SupplierAccount>()
                .Where(w => w.SupplierId == supplierId && w.Status == (int)EnumStatus.Active)
                .ToListAsync();
            supplierDetails.supplierProducts = await _context.Set<SupplierProduct>()
               .Where(w => w.SupplierId == supplierId && w.Status == (int)EnumStatus.Active)
               .ToListAsync();
            return supplierDetails;
        }

        public async Task<Supplier> GetSupplierByNameAsync(string name)
        {
            return await _context.Set<Supplier>()
                .Where(w => w.SupplierName.ToUpper() == name.ToUpper() && w.Status == (int)EnumStatus.Active)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Supplier>> GetSuppliersByPagingAsync(GetPagedSearch request)
        {
            var query = _context.Set<Supplier>().AsQueryable();

            if (!string.IsNullOrEmpty(request.searchText))
            {
                if (int.TryParse(request.searchText, out int supplierId))
                {
                    // If numeric → search by ID
                    query = query.Where(w => w.SupplierId == supplierId);
                }
                else
                {
                    query = query.Where(w => w.SupplierName.Contains(request.searchText));
                }
            }
            query = query.Where(w => w.Status != (int)EnumStatus.Deleted);
            var result = await query
                .OrderBy(o => o.SupplierName)
                .Skip((request.pageNo - 1) * request.pageSize)
                .Take(request.pageSize)
                .ToListAsync();

            return result;
        }

        public async Task<int> GetTotalSuppliersCountAsync(GetPagedSearch request)
        {
            var query = _context.Set<Supplier>().AsQueryable();

            if (!string.IsNullOrEmpty(request.searchText))
            {
                query = query.Where(w => w.SupplierName.Contains(request.searchText));
            }
            return await query.CountAsync();
        }

        public async Task<List<SupplierAccount>> GetSupplierAccountsByIdAsync(int supplierId)
        {
            return await _context.Set<SupplierAccount>()
                .Where(w => w.SupplierId == supplierId && w.Status == (int)EnumStatus.Active)
                .ToListAsync();
        }

        public async Task<SupplierAccount> GetSupplierAccountByIdAsync(int supplierAccountId)
        {
            return await _context.Set<SupplierAccount>()
                .Where(w => w.SupplierAccountId == supplierAccountId && w.Status == (int)EnumStatus.Active)
                .FirstOrDefaultAsync();
        }

    }
}
