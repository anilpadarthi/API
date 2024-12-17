using SIMAPI.Business.Interfaces;
using SIMAPI.Data.Models;

namespace SIMAPI.Business.Services
{
    public class MappingService : IMappingService
    {
        Task<CommonResponse> IMappingService.MapAreasToUserAsync(int[] selectedIds, int mappedTo)
        {
            throw new NotImplementedException();
        }

        Task<CommonResponse> IMappingService.MapLotNoToUserAsync(int[] selectedIds, int mappedTo)
        {
            throw new NotImplementedException();
        }

        Task<CommonResponse> IMappingService.MapUsersToManagerAsync(int[] selectedIds, int mappedTo)
        {
            throw new NotImplementedException();
        }
    }
}
