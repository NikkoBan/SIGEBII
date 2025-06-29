using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SIGEBI.Application.Contracts.Repository;
using SIGEBI.Domain.Base;
using SIGEBI.Persistence.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Persistence.Base
{
    namespace SIGEBI.Persistence.Repositories
    {
        public abstract class BaseRepositoryEf<TEntity> : IBaseRepository<TEntity>
            where TEntity : class
        {
            protected readonly SIGEBIContext _context;
            protected readonly DbSet<TEntity> _dbSet;
            protected readonly ILogger<BaseRepositoryEf<TEntity>> _logger;
            public BaseRepositoryEf(SIGEBIContext context, ILogger<BaseRepositoryEf<TEntity>> logger)
            {
                _context = context;
                _dbSet = context.Set<TEntity>();
                _logger = logger;
            }

            public virtual Task<OperationResult> CreateAsync(TEntity entity)
            {
                throw new NotImplementedException();
            }

            public Task<OperationResult> CreateAsync(AuditableEntity entity)
            {
                throw new NotImplementedException();
            }

            public virtual Task<OperationResult> DeleteAsync(int id)
            {
                throw new NotImplementedException();
            }

            public virtual Task<bool> ExistsAsync(int id)
            {
                throw new NotImplementedException();
            }

            public virtual Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter)
            {
                throw new NotImplementedException();
            }

            public virtual Task<OperationResult> GetAllAsync(Expression<Func<TEntity, bool>> filter)
            {
                throw new NotImplementedException();
            }

            public Task<OperationResult> GetAllAsync()
            {
                throw new NotImplementedException();
            }

            public virtual Task<OperationResult> GetByIdAsync(int id)
            {
                throw new NotImplementedException();
            }

            public virtual Task<OperationResult> UpdateAsync(TEntity entity)
            {
                throw new NotImplementedException();
            }
        }
    }
}


