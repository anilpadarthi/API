using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Models.Topup;
using SIMAPI.Repository.Interfaces;

namespace SIMAPI.Business.Services
{
    public class TopupService : ITopupService
    {
        private readonly ITopupRepository _repository;
        public TopupService(ITopupRepository repository)
        {
            _repository = repository;
        }

        public async Task<TopupResponse> ValidateIMEI(string phoneNo, string shopId, string topupAmount)
        {
            return await _repository.ValidateIMEI(phoneNo, shopId, topupAmount);
        }

        public async Task<bool> SaveTopup(string phoneNo, string shopId, string topupAmount)
        {
            return await _repository.SaveTopup(phoneNo, shopId, topupAmount);
        }
    }
}
