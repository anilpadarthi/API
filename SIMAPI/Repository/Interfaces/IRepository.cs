using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace SIMAPI.Repository.Interfaces
{
    public interface IRepository
    {
        Task<List<dynamic>> GetDataTable(string procedureName, params DbParameter[] parameters);
        Task<int> SaveChangesAsync();
        Task LogError(Exception ex);
        void Add<TEntity>(TEntity record) where TEntity : class;
        void Update<TEntity>(TEntity record) where TEntity : class;
        void Remove<TEntity>(TEntity record) where TEntity : class;
        IEnumerable<string> GetDirtyPropertyList<TEntity>(TEntity entity) where TEntity : class;
        void DetachEntity<TEntity>(TEntity entity) where TEntity : class;
    }
}
