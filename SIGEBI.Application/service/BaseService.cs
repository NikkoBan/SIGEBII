
using SIGEBI.Domain.Base;
using AutoMapper;
using Microsoft.Extensions.Logging;
using SIGEBI.Application.Contracts.Service;
using SIGEBI.Domain.IRepository;
using Microsoft.EntityFrameworkCore;


namespace SIGEBI.Application.Services
{

    public abstract class BaseService<TDto, TCreateDto, TUpdateDto, TEntity>
        : IBaseService<TDto, TCreateDto, TUpdateDto>
        where TEntity : AuditableEntity
        where TDto : class
        where TCreateDto : class
        where TUpdateDto : class
    {
        protected readonly IBaseRepository<TEntity> _repository;
        protected readonly ILogger _logger;

        protected BaseService(IBaseRepository<TEntity> repository, ILogger logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task<OperationResult> GetAllAsync()
        {
            try
            {
                var result = await _repository.GetAllAsync();
                if (!result.Success) return result;

                var dtos = MapToDtoList((IEnumerable<TEntity>)result.Data!);
                return OperationResult.SuccessResult(dtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener todos los registros de {typeof(TEntity).Name}");
                return OperationResult.FailureResult($"Error interno: {ex.Message}");
            }
        }

        public async Task<OperationResult> GetByIdAsync(int id)
        {
            try
            {
                var result = await _repository.GetByIdAsync(id);
                if (!result.Success || result.Data == null)
                    return OperationResult.FailureResult($"{typeof(TEntity).Name} no encontrado.");

                var dto = MapToDto((TEntity)result.Data);
                return OperationResult.SuccessResult(dto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener {typeof(TEntity).Name} con ID {id}");
                return OperationResult.FailureResult($"Error interno: {ex.Message}");
            }
        }

        public virtual async Task<OperationResult> CreateAsync(TCreateDto dto)
        {
            try
            {
                var entity = MapToEntity(dto);

                entity.CreatedAt = DateTime.UtcNow;
                entity.CreatedBy = "System";
                entity.UpdatedAt = DateTime.UtcNow;
                entity.UpdatedBy = "System";
                entity.IsDeleted = false;

                var result = await _repository.CreateAsync(entity);
                return result;
            }
            catch (DbUpdateException dbEx)
            {
                _logger.LogError(dbEx, $"Error al crear {typeof(TEntity).Name}: {dbEx.InnerException?.Message ?? dbEx.Message}");
                return OperationResult.FailureResult($"Error al crear {typeof(TEntity).Name}: {dbEx.InnerException?.Message ?? dbEx.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al crear {typeof(TEntity).Name}");
                return OperationResult.FailureResult($"Error interno: {ex.Message}");
            }

        }
        public async Task<OperationResult> UpdateAsync(int id, TUpdateDto dto)
        {
            try
            {
                var existingResult = await _repository.GetByIdAsync(id);
                if (!existingResult.Success || existingResult.Data is not TEntity entity)
                    return OperationResult.FailureResult($"{typeof(TEntity).Name} no encontrado.");

                var updatedEntity = MapToEntity(dto, entity);

                // Aquí seteas la auditoría de actualización
                updatedEntity.UpdatedAt = DateTime.UtcNow;
                updatedEntity.UpdatedBy = ObtenerUsuarioActual();

                return await _repository.UpdateAsync(updatedEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar {typeof(TEntity).Name} con ID {id}");
                return OperationResult.FailureResult($"Error interno: {ex.Message}");
            }
        }

        public async Task<OperationResult> DeleteAsync(int id)
        {
            try
            {
                return await _repository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar {typeof(TEntity).Name} con ID {id}");
                return OperationResult.FailureResult($"Error interno: {ex.Message}");
            }
        }
        protected virtual string ObtenerUsuarioActual()
        {
            return "System";
        }


        // =======================
        // Métodos de mapeo abstractos
        // =======================

        protected abstract TDto MapToDto(TEntity entity);
        protected abstract IEnumerable<TDto> MapToDtoList(IEnumerable<TEntity> entities);
        protected abstract TEntity MapToEntity(TCreateDto dto);
        protected abstract TEntity MapToEntity(TUpdateDto dto, TEntity entity);
    }
}


