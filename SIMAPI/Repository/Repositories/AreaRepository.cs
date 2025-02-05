using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SIMAPI.Business.Enums;
using SIMAPI.Data;
using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models;
using SIMAPI.Repository.Interfaces;

namespace SIMAPI.Repository.Repositories
{
    public class AreaRepository : Repository, IAreaRepository
    {
        public AreaRepository(SIMDBContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Area>> GetAllAreasAsync()
        {
            return await _context.Set<Area>()
                .Where(w => w.Status != (short)EnumStatus.Deleted)
                .ToListAsync();
        }

        public async Task<Area> GetAreaByIdAsync(int id)
        {
            return await _context.Set<Area>()
                .Where(w => w.AreaId == id)
                .FirstOrDefaultAsync();
        }

        public async Task<Area> GetAreaByNameAsync(string name)
        {
            return await _context.Set<Area>()
                .Where(w => w.AreaName.ToUpper() == name.ToUpper())
                .Where(w => w.Status != (short)EnumStatus.Deleted)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Area>> GetAreasByPagingAsync(GetPagedSearch request)
        {
            var query = _context.Set<Area>().AsQueryable();
            query = query.Where(w => w.Status != (short)EnumStatus.Deleted);

            if (!string.IsNullOrEmpty(request.searchText))
            {
                query = query.Where(w => w.AreaName.Contains(request.searchText));
            }


            var result = await query
                .OrderBy(o => o.AreaName)
                .Skip((request.pageNo - 1) * request.pageSize)
                .Take(request.pageSize)
                .ToListAsync();

            return result;
        }

        public async Task<int> GetTotalAreasCountAsync(GetPagedSearch request)
        {
            var query = _context.Set<Area>().AsQueryable();
            query = query.Where(w => w.Status != (short)EnumStatus.Deleted);

            if (!string.IsNullOrEmpty(request.searchText))
            {
                query = query.Where(w => w.AreaName.Contains(request.searchText));
            }
            return await query.CountAsync();
        }

        public async Task<AreaMap> GetAreaMapByAreaIdAsync(int areaId)
        {
            return await _context.Set<AreaMap>()
                .Where(w => w.AreaId == areaId && w.IsActive == true)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<AllocateAreaDetails>> GetAllAreasToAllocateAsync()
        {
            return await ExecuteStoredProcedureAsync<AllocateAreaDetails>("exec [dbo].[Get_All_Areas_To_Allocate]");
        }

        public async Task<IEnumerable<AreaAllocationHistory>> ViewAreaAllocationHistorySync(int areaId)
        {
            var paramList = new[]
            {
                    new SqlParameter("@areaId", areaId),
            };
            return await ExecuteStoredProcedureAsync<AreaAllocationHistory>("exec [dbo].[Get_Area_Allocate_History] @areaId", paramList);
        }
    }
}
