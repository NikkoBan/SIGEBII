using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using SIGEBI.Application.Contracts.Repository;

using SIGEBI.Domain.Base;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SIGEBI.Persistence.Repositories
{
    namespace SIGEBI.Persistence.Repositories
    {
        public abstract class BaseRepositoryAdo<TEntity> : IBaseRepository<TEntity>
            where TEntity : class
        {
            protected readonly string _connectionString;
            protected readonly ILogger<BaseRepositoryAdo<TEntity>> _logger;

            public BaseRepositoryAdo(string connectionString, ILogger<BaseRepositoryAdo<TEntity>> logger)
            {
                _connectionString = connectionString;
                _logger = logger;
            }

            public abstract Task<OperationResult> CreateAsync(TEntity entity);
            public abstract Task<OperationResult> CreateAsync(AuditableEntity entity);
            public abstract Task<OperationResult> DeleteAsync(int id);
            public abstract Task<bool> ExistsAsync(int id);
            public abstract Task<OperationResult> GetAllAsync();
            public abstract Task<OperationResult> GetByIdAsync(int id);
            public abstract Task<OperationResult> UpdateAsync(TEntity entity);
        }
    }
}