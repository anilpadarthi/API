using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;

namespace SIMAPI.Repository.Interfaces
{
    public interface INetworkRepository : IRepository
    {
        Task<Network> GetNetworkByIdAsync(int id);
        Task<Network> GetNetworkByNameAsync(string name,string skuCode);
        Task<IEnumerable<Network>> GetAllNetworksAsync();
        Task<IEnumerable<Network>> GetNetworksByPagingAsync(GetPagedSearch request);
        Task<int> GetTotalNetworksCountAsync(GetPagedSearch request);
    }
}
