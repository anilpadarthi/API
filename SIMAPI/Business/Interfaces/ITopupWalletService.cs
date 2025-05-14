using SIMAPI.Data.Models;

namespace SIMAPI.Business.Interfaces
{
    public interface ITopupWalletService
    {

        Task<string> LoginAsync(string username, string password);
        Task<string> UpdateBalanceAsync(string token, BalanceUpdateRequest balanceUpdate);


    }
}
