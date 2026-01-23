using SIMAPI.Data.Dto;
using SIMAPI.Data.Entities;
using SIMAPI.Data.Models;

namespace SIMAPI.Repository.Interfaces
{
    public interface ICommonRepository : IRepository
    {
        Task LogError(Exception ex, string optional = "");
    }
}
