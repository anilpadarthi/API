using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models;

namespace SIMAPI.Repository.Interfaces
{
    public interface IAreaRepository : IRepository
    {
        Task<Area> GetAreaByIdAsync(int id);
        Task<Area> GetAreaByNameAsync(string name);
        Task<IEnumerable<Area>> GetAllAreasAsync();       
        Task<IEnumerable<Area>> GetAreasByPagingAsync(GetPagedSearch request);
        Task<int> GetTotalAreasCountAsync(GetPagedSearch request);
        Task<AreaMap> GetAreaMapByAreaIdAsync(int areaId);

        Task<IEnumerable<AllocateAreaDetails>> GetAllAreasToAllocateAsync();
    }
}
