using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using SIGEBI.Application.Contracts.Repository;
using SIGEBI.Application.Contracts.Repository.SIGEBI.Persistence.Repositories;
using SIGEBI.Domain.Base;
using System.Data;
using System.Linq.Expressions;

namespace SIGEBI.Persistence.Repositories
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class, new()
    {
        protected readonly string _connection;
        private readonly ILogger<BaseRepository<TEntity>> _logger;
        private readonly string _entityName;

        public BaseRepository(string connection, ILogger<BaseRepository<TEntity>> logger)
        {
            _connection = connection;

            _logger = logger;
            _entityName = typeof(TEntity).Name;

        }

        public BaseRepository(string connection)
        {
            _connection = connection;
            _entityName = typeof(TEntity).Name;
        }

        public async Task<OperationResult> CreateAsync(TEntity entity)
        {
            var procedure = $"Create{_entityName}";
            return await ExecuteNonQueryAsync(procedure, entity, hasOutput: true);
        }

        public async Task<OperationResult> GetByIdAsync(int id)
        {
            var procedure = $"Get{_entityName}ById";
            return await ExecuteReaderAsync(procedure, new Dictionary<string, object> { { $"{_entityName}Id", id } });
        }

        public async Task<OperationResult> GetAllAsync()
        {
            var procedure = $"Get{_entityName}s";
            return await ExecuteReaderAsync(procedure);
        }

        public async Task<OperationResult> UpdateAsync(TEntity entity)
        {
            var procedure = $"Modify{_entityName}";
            return await ExecuteNonQueryAsync(procedure, entity);
        }

        public async Task<OperationResult> DeleteAsync(int id)
        {
            var procedure = $"Disable{_entityName}";
            return await ExecuteNonQueryAsync(procedure, new Dictionary<string, object> { { $"{_entityName}Id", id }, { "DeletedBy", "system" } });
        }

        public Task<OperationResult> GetAllAsync(Expression<Func<TEntity, bool>> filter)
        {
            return Task.FromResult(new OperationResult { Success = false, Message = "Filtered queries not implemented" });
        }

        public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter)
        {
            return Task.FromResult(false); // No implementado para SPs
        }

        private async Task<OperationResult> ExecuteReaderAsync(string procedure, Dictionary<string, object> parameters = null)
        {
            try
            {
                using (var connection = new SqlConnection(_connection))
                using (var command = new SqlCommand(procedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    if (parameters != null)
                    {
                        foreach (var param in parameters)
                        {
                            command.Parameters.AddWithValue($"@{param.Key}", param.Value ?? DBNull.Value);
                        }
                    }

                    await connection.OpenAsync();
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        var list = new List<TEntity>();
                        while (await reader.ReadAsync())
                        {
                            var entity = new TEntity();
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var prop = typeof(TEntity).GetProperty(reader.GetName(i));
                                if (prop != null && reader[i] != DBNull.Value)
                                {
                                    prop.SetValue(entity, reader[i]);
                                }
                            }
                            list.Add(entity);
                        }

                        return new OperationResult { Success = true, Data = list }; // Siempre devuelve lista
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Error in {procedure}");
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }

        private async Task<OperationResult> ExecuteNonQueryAsync(string procedure, object obj, bool hasOutput = false)
        {
            try
            {
                using (var connection = new SqlConnection(_connection))
                using (var command = new SqlCommand(procedure, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    foreach (var prop in obj.GetType().GetProperties())
                    {
                        if (prop.Name == "P_result" || prop.Name == "p_result") continue; // Evitar conflicto con output
                        command.Parameters.AddWithValue($"@{prop.Name}", prop.GetValue(obj) ?? DBNull.Value);
                    }

                    if (hasOutput)
                    {
                        var output = new SqlParameter("@P_result", SqlDbType.VarChar, 1000)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(output);
                    }

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();

                    string resultMsg = hasOutput ? command.Parameters["@P_result"].Value.ToString() : "OK";
                    return new OperationResult { Success = true, Message = resultMsg };
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Error in {procedure}");
                return new OperationResult { Success = false, Message = ex.Message };
            }
        }
    }
}

