using SIMAPI.Data.Models;

namespace SIMAPI.Business.Interfaces
{
    public interface IMappingService
    {

        Task<CommonResponse> MapAreasToUserAsync(int[] selectedIds, int mappedTo);
        Task<CommonResponse> MapUsersToManagerAsync(int[] selectedIds, int mappedTo);
        Task<CommonResponse> MapLotNoToUserAsync(int[] selectedIds, int mappedTo);
    }
}
