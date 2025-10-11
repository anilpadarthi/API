using Microsoft.EntityFrameworkCore;
using SIMAPI.Business.Enums;
using SIMAPI.Data;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Repository.Interfaces;

namespace SIMAPI.Repository.Repositories
{
    public class MixMatchGroupRepository : Repository, IMixMatchGroupRepository
    {
        public MixMatchGroupRepository(SIMDBContext context) : base(context)
        {
        }





        public async Task<MixMatchGroup> GetMixMatchGroupByIdAsync(int MixMatchGroupId)
        {
            return await _context.Set<MixMatchGroup>()
                .Where(w => w.MixMatchGroupId == MixMatchGroupId)
                .FirstOrDefaultAsync();

        }

        public async Task<MixMatchGroup> GetMixMatchGroupByNameAsync(string name)
        {
            return await _context.Set<MixMatchGroup>()
                .Where(w => w.GroupName.ToUpper() == name.ToUpper())
                .Where(w => w.Status != (short)EnumStatus.Deleted)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<MixMatchGroup>> GetMixMatchGroupsByPagingAsync(GetPagedSearch request)
        {
            var query = _context.Set<MixMatchGroup>().AsQueryable();
            query = query.Where(w => w.Status != (short)EnumStatus.Deleted);

            if (!string.IsNullOrEmpty(request.searchText))
            {
                if (int.TryParse(request.searchText, out int mixMatchGroupId))
                {
                    // If numeric → search by ID
                    query = query.Where(w => w.MixMatchGroupId == mixMatchGroupId);
                }
                else
                {
                    query = query.Where(w => w.GroupName.Contains(request.searchText));
                }
            }


            var result = await query
                .OrderBy(o => o.GroupName)
                .Skip((request.pageNo - 1) * request.pageSize)
                .Take(request.pageSize)
                .ToListAsync();

            return result;
        }

        public async Task<int> GetTotalMixMatchGroupsCountAsync(GetPagedSearch request)
        {
            var query = _context.Set<MixMatchGroup>().AsQueryable();
            query = query.Where(w => w.Status != (short)EnumStatus.Deleted);

            if (!string.IsNullOrEmpty(request.searchText))
            {
                query = query.Where(w => w.GroupName.Contains(request.searchText));
            }
            return await query.CountAsync();
        }


    }
}
