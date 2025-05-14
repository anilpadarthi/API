using SIMAPI.Data.Models.Topup;

namespace SIMAPI.Business.Interfaces
{
    public interface ITopupService
    {

        Task<TopupResponse> ValidateIMEI(string phoneNo, string shopId, string topupAmount);
        Task<bool> SaveTopup(string phoneNo, string shopId, string topupAmount);


    }
}
