using SIMAPI.Data.Models.Topup;

namespace SIMAPI.Repository.Interfaces
{
    public interface ITopupRepository : IRepository
    {
        Task<TopupResponse> ValidateIMEI(string phoneNo, string shopId, string topupAmount);
        Task<bool> SaveTopup(string phoneNo, string shopId, string topupAmount);
    }
}
