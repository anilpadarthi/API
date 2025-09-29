using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;

namespace SIMAPI.Repository.Interfaces
{
    public interface IMixMatchGroupRepository : IRepository
    {
        Task<MixMatchGroup> GetMixMatchGroupByIdAsync(int MixMatchGroupId);
        Task<MixMatchGroup> GetMixMatchGroupByNameAsync(string name);
        Task<IEnumerable<MixMatchGroup>> GetMixMatchGroupsByPagingAsync(GetPagedSearch request);
        Task<int> GetTotalMixMatchGroupsCountAsync(GetPagedSearch request);
    }
}
