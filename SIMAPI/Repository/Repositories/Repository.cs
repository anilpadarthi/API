using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SIMAPI.Data;
using SIMAPI.Data.Entities;
using SIMAPI.Repository.Interfaces;
using System.Data;
using System.Data.Common;
using System.Dynamic;

namespace SIMAPI.Repository.Repositories
{
    public class Repository : IRepository
    {
        protected readonly SIMDBContext _context;

        public Repository(SIMDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TEntity>> ExecuteStoredProcedureAsync<TEntity>(string storedProcedure) where TEntity : class
        {
            var list = await _context
                .Set<TEntity>()
                .FromSqlRaw(storedProcedure)
                .ToListAsync();

            return list;
        }

        public async Task<IEnumerable<TEntity>> ExecuteStoredProcedureAsync<TEntity>(string storedProcedure, params SqlParameter[] sqlParameters) where TEntity : class
        {
            var list = await _context
                .Set<TEntity>()
                .FromSqlRaw(storedProcedure, sqlParameters)
                .ToListAsync();

            return list;
        }

        public async Task<List<TEntity>> ExecutePrimitiveStoredProcedureAsync<TEntity>(string storedProcedure, params SqlParameter[] sqlParameters) where TEntity : class
        {
            var list = await _context.Database.SqlQueryRaw<TEntity>($"{storedProcedure}", sqlParameters).ToListAsync();

            return list;
        }

        public async Task<TEntity> ExecuteStoredProcedureReturnsFirstItemAsync<TEntity>(string storedProcedure, params SqlParameter[] sqlParameters) where TEntity : class
        {
            var list = await _context
                .Set<TEntity>()
                .FromSqlRaw(storedProcedure, sqlParameters)
                .FirstOrDefaultAsync();

            return list;
        }


        public async Task<List<dynamic>> GetDataTable(string procedureName, params DbParameter[] parameters)
        {
            var connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString);
            using (var cmd = new SqlCommand(procedureName))
            {
                cmd.Connection = connection;
                cmd.CommandType = CommandType.StoredProcedure;
                if (parameters != null)
                {
                    foreach (var item in parameters)
                    {
                        cmd.Parameters.Add(item);
                    }
                }
                connection.Open();
                var reader = await cmd.ExecuteReaderAsync();
                List<dynamic> results = DataReaderToDynamicList(reader);
                connection.Close();
                return results;
            }
            return null;
        }

        public async Task<List<dynamic>> GetDataSet(string procedureName, params DbParameter[] parameters)
        {
            var connectionString = _context.Database.GetDbConnection().ConnectionString;
            var results = new List<dynamic>();

            using (var connection = new SqlConnection(connectionString))
            {
                using (var cmd = new SqlCommand(procedureName))
                {
                    cmd.Connection = connection;
                    cmd.CommandType = CommandType.StoredProcedure;
                    if (parameters != null)
                    {
                        foreach (var item in parameters)
                        {
                            cmd.Parameters.Add(item);
                        }
                    }

                    // Use SqlDataAdapter to fill a DataSet
                    using (var adapter = new SqlDataAdapter(cmd))
                    {
                        var dataSet = new DataSet();
                        await Task.Run(() => adapter.Fill(dataSet)); // Fill DataSet asynchronously

                        // Convert each DataTable in the DataSet to a dynamic list
                        foreach (DataTable table in dataSet.Tables)
                        {
                            results.Add(ConvertDataTableToDynamicList(table));
                        }
                    }
                }

            }
            return results;
        }

        private static List<dynamic> DataReaderToDynamicList(SqlDataReader reader)
        {
            var result = new List<dynamic>();

            while (reader.Read())
            {
                var expandoObject = new ExpandoObject() as IDictionary<string, Object>;
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    expandoObject.Add(reader.GetName(i), reader[i].ToString() == "" ? 0 : reader[i].ToString());
                }
                result.Add(expandoObject);
            }

            return result;
        }

        private List<dynamic> ConvertDataTableToDynamicList(DataTable table)
        {
            var dynamicList = new List<dynamic>();

            foreach (DataRow row in table.Rows)
            {
                var dynamicObject = new ExpandoObject() as IDictionary<string, object>;
                foreach (DataColumn column in table.Columns)
                {
                    dynamicObject[column.ColumnName] = row[column] == DBNull.Value ? null : row[column];
                }
                dynamicList.Add(dynamicObject);
            }

            return dynamicList;
        }



        public async Task<int> ExecuteStoredProcedureAsync(string storedProcedure)
        {
            int result = await _context.Database.ExecuteSqlRawAsync(storedProcedure);
            return result;
        }

        public async Task<int> ExecuteStoredProcedureAsync(string storedProcedure, params SqlParameter[] sqlParameters)
        {
            int result = await _context.Database.ExecuteSqlRawAsync(storedProcedure, sqlParameters);
            return result;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Add<TEntity>(TEntity record) where TEntity : class
        {
            _context.Set<TEntity>().Add(record);
        }

        public void Update<TEntity>(TEntity record) where TEntity : class
        {
            _context.Set<TEntity>().Update(record);
        }

        public void Remove<TEntity>(TEntity record) where TEntity : class
        {
            _context.Set<TEntity>().Remove(record);
        }

        public IEnumerable<string> GetDirtyPropertyList<TEntity>(TEntity entity) where TEntity : class
        {
            var modifiedPropertyList = new List<string>();
            var entityEntry = _context.Entry(entity);

            foreach (var property in entityEntry.CurrentValues.Properties)
            {
                var propertyEntry = entityEntry.Property(property.Name);
                if (!propertyEntry.IsModified)
                    continue;

                if (IsSame(propertyEntry.OriginalValue, propertyEntry.CurrentValue))
                    continue;

                modifiedPropertyList.Add(property.Name);
            }

            return modifiedPropertyList;
        }

        public void DetachEntity<TEntity>(TEntity entity) where TEntity : class
        {
            _context.Entry(entity).State = EntityState.Detached;
        }

        public async Task LogError(Exception ex)
        {
            var errorLog = new ErrorInfo
            {
                ErrorMessage = ex.Message,
                StackTrace = ex.StackTrace,
                Method = ex.Source,
                CreatedDate = DateTime.Now
            };

            _context.Add(errorLog);
            await _context.SaveChangesAsync();
        }

        private bool IsSame(object? a, object? b)
        {
            if (a == null && b == null)
                return true;

            if (a == null && b != null)
                return false;

            if (a != null && b == null)
                return false;

            return a.Equals(b);
        }
    }
}
