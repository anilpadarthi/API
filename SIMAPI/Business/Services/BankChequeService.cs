using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SIMAPI.Business.Services
{
    public class BankChequeService : IBankChequeService
    {
        Task<CommonResponse> IBankChequeService.GetShopCommissionChequeDetailsAsync(int shopId)
        {
            throw new NotImplementedException();
        }

        Task<CommonResponse> IBankChequeService.ReplaceShopCommissionCheque(int shopId)
        {
            throw new NotImplementedException();
        }

        Task<CommonResponse> IBankChequeService.SaveNewShopCommissionCheque(int shopId)
        {
            throw new NotImplementedException();
        }
    }
}
