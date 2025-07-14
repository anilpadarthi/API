using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models.Sim;
using System.Text;

namespace SIMAPI.Repository.Interfaces
{
    public interface ISimRepository : IRepository
    {
        Task<bool> IsValidSim(string IMEI);
        Task<IEnumerable<SimScanModel>> ScanSimsAsync(StringBuilder simNumbersBuilder);
        Task<Sim?> GetSimDetailsAsync(string IMEI);
        Task<SimMap?> GetSimMapDetailsAsync(int simId);
        Task<IEnumerable<SimHistoryModel>> GetSimHistoryDetailsAsync(StringBuilder simNumbersBuilder);
        Task DeAllocateFromSyncSimAPI(int simId);
    }
}
