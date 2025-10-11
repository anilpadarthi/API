using Microsoft.EntityFrameworkCore;
using SIMAPI.Business.Enums;
using SIMAPI.Data;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Repository.Interfaces;

namespace SIMAPI.Repository.Repositories
{
    public class NetworkRepository : Repository, INetworkRepository
    {
        public NetworkRepository(SIMDBContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Network>> GetAllNetworksAsync()
        {
            return await _context.Set<Network>()
                .Where(w => w.Status != (short)EnumStatus.Deleted)
                .ToListAsync();
        }

        public async Task<Network> GetNetworkByIdAsync(int id)
        {
            return await _context.Set<Network>()
                .Where(w => w.NetworkId == id)
                .FirstOrDefaultAsync();
        }

        public async Task<BaseNetwork> GetBaseNetworkByIdAsync(int id)
        {
            return await _context.Set<BaseNetwork>()
                .Where(w => w.BaseNetworkId == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Network> GetNetworkByNameAsync(string name, string skuCode)
        {
            return await _context.Set<Network>()
                .Where(w => w.SkuCode.ToUpper() == skuCode.ToUpper())
                .Where(w => w.Status != (short)EnumStatus.Deleted)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Network>> GetNetworksByPagingAsync(GetPagedSearch request)
        {
            var query = _context.Set<Network>().AsQueryable();
            query = query.Where(w => w.Status != (short)EnumStatus.Deleted);

            if (!string.IsNullOrEmpty(request.searchText))
            {
                if (int.TryParse(request.searchText, out int networkId))
                {
                    // If numeric → search by ID
                    query = query.Where(w => w.NetworkId == networkId);
                }
                else
                {
                    query = query.Where(w => w.NetworkName.Contains(request.searchText));
                }
            }

            var result = await query
                .OrderBy(o => o.NetworkName)
                .Skip((request.pageNo - 1) * request.pageSize)
                .Take(request.pageSize)
                .ToListAsync();

            return result;
        }

        public async Task<int> GetTotalNetworksCountAsync(GetPagedSearch request)
        {
            var query = _context.Set<Network>().AsQueryable();
            query = query.Where(w => w.Status != (short)EnumStatus.Deleted);

            if (!string.IsNullOrEmpty(request.searchText))
            {
                query = query.Where(w => w.NetworkName.Contains(request.searchText));
            }
            return await query.CountAsync();
        }


    }
}
