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


        public async Task<List<dynamic>> GetDataTableAsync(string procedureName, params DbParameter[] parameters)
        {
            await using var connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString);
            await using var cmd = new SqlCommand(procedureName, connection);

            cmd.CommandType = CommandType.StoredProcedure;

            if (parameters != null)
            {
                foreach (var param in parameters)
                {
                    cmd.Parameters.Add(param);
                }
            }

            await connection.OpenAsync();

            await using var reader = await cmd.ExecuteReaderAsync();

            var results = await DataReaderToDynamicListAsync(reader);

            return results;
        }

        public async Task<List<List<dynamic>>> GetDataSetAsync(string procedureName, params DbParameter[] parameters)
        {
            var results = new List<List<dynamic>>();

            var connection = _context.Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync();

            using var cmd = connection.CreateCommand();
            cmd.CommandText = procedureName;
            cmd.CommandType = CommandType.StoredProcedure;

            if (parameters != null)
            {
                foreach (var param in parameters)
                    cmd.Parameters.Add(param);
            }

            using var reader = await cmd.ExecuteReaderAsync();

            do
            {
                var tableResult = new List<dynamic>();

                while (await reader.ReadAsync())
                {
                    IDictionary<string, object> row = new ExpandoObject();

                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                    }

                    tableResult.Add(row);
                }

                results.Add(tableResult);

            } while (await reader.NextResultAsync());

            return results;
        }

        private static async Task<List<dynamic>> DataReaderToDynamicListAsync(SqlDataReader reader)
        {
            var result = new List<dynamic>();

            while (await reader.ReadAsync())
            {
                IDictionary<string, object> row = new ExpandoObject();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var value = reader.IsDBNull(i) ? null : reader.GetValue(i);

                    row[reader.GetName(i)] = value ?? "0";
                }

                result.Add(row);
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

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
                return 1;
            }
            catch (Exception ex)
            {
                //LogService(ex.Message);
                return 0;

            }
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

        public async Task LogError(Exception ex, string optional = "")
        {
            var errorLog = new ErrorInfo
            {
                ErrorMessage = ex.Message + "_" + optional,
                StackTrace = ex.StackTrace,
                Method = ex.Source,
                CreatedDate = DateTime.Now
            };

            _context.Add(errorLog);
            await _context.SaveChangesAsync();
        }

        public async Task<object?> GetScalar(string procedureName, params DbParameter[] parameters)
        {
            var connectionString = _context.Database.GetDbConnection().ConnectionString;

            await using var connection = new SqlConnection(connectionString);
            await using var cmd = new SqlCommand(procedureName, connection);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 120; // optional safety

            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters);
            }

            await connection.OpenAsync();
            return await cmd.ExecuteScalarAsync();
        }

        private void LogService(string content)
        {
            // Replace ConfigurationManager.AppSettings with Environment.GetEnvironmentVariable or another configuration source
            // Example assumes you have set an environment variable named "ErrorLogPath"
            var path = "G:\\ExceptionLog.txt";
            FileStream fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs);
            sw.BaseStream.Seek(0, SeekOrigin.End);
            sw.WriteLine(content);
            sw.Flush();
            sw.Close();
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
