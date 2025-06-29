using SIGEBI.Application.Contracts.Repository;
using SIGEBI.Application.Contracts.Service;
using SIGEBI.Domain.Base;
using AutoMapper;

namespace SIGEBI.Application.Services
{
    public abstract class BaseService<TEntity, TCreateDto, TUpdateDto, TReadDto> : IBaseService<TCreateDto, TUpdateDto, TReadDto>
        where TEntity : AuditableEntity
        where TCreateDto : class
        where TUpdateDto : class
        where TReadDto : class
    {



        protected readonly IBaseRepository<TEntity> _repository;
        protected readonly IMapper _mapper;

       
        public BaseService(IBaseRepository<TEntity> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
      


        public virtual async Task<OperationResult> CreateAsync(TCreateDto dto)
        {
            try
            {
                var entity = _mapper.Map<TEntity>(dto);
                entity.CreatedAt = DateTime.UtcNow;
                entity.CreatedBy = "System";

                var result = await _repository.CreateAsync(entity);
                if (result.Success)
                {
                    var readDto = _mapper.Map<TReadDto>(entity);
                    return new OperationResult { Success = true, Data = readDto };
                }

                return new OperationResult { Success = false, Message = result.Message };
            }
            catch (Exception ex)
            {
                return new OperationResult { Success = false, Message = $"Error: {ex.Message}" };
            }
        }


        public virtual async Task<OperationResult> DeleteAsync(int id)
        {
            try
            {
                var result = await _repository.DeleteAsync(id);
                return result;
            }
            catch (Exception ex)
            {
                return new OperationResult { Success = false, Message = $"Error: {ex.Message}" };
            }
        }
        



        public virtual async Task<OperationResult> GetAllAsync()
        {
            try
            {
                var result = await _repository.GetAllAsync();
                if (result.Success && result.Data is IEnumerable<TEntity> list)
                {
                    var dtoList = _mapper.Map<IEnumerable<TReadDto>>(list);
                    return new OperationResult { Success = true, Data = dtoList };
                }

                return new OperationResult { Success = false, Message = result.Message ?? "No se pudo obtener la lista." };
            }
            catch (Exception ex)
            {
                return new OperationResult { Success = false, Message = $"Error: {ex.Message}" };
            }
        }

        public virtual async Task<OperationResult> GetByIdAsync(int id)
        {
            try
            {
                var result = await _repository.GetByIdAsync(id);
                if (result.Success && result.Data is TEntity entity)
                {
                    var dto = _mapper.Map<TReadDto>(entity);
                    return new OperationResult { Success = true, Data = dto };
                }

                return new OperationResult { Success = false, Message = $"Entidad con ID {id} no encontrada." };
            }
            catch (Exception ex)
            {
                return new OperationResult { Success = false, Message = $"Error: {ex.Message}" };
            }
        }

        public virtual async Task<OperationResult> UpdateAsync(int id, TUpdateDto dto)
        {
            try
            {
                var result = await _repository.GetByIdAsync(id);
                if (!result.Success || result.Data is not TEntity entity)
                {
                    return new OperationResult { Success = false, Message = $"No se encontró la entidad con ID {id} para actualizar." };
                }

                _mapper.Map(dto, entity);
                entity.UpdateAt = DateTime.UtcNow;
                entity.UpdateBy = "System";

                var updateResult = await _repository.UpdateAsync(entity);
                if (updateResult.Success)
                {
                    var updatedDto = _mapper.Map<TReadDto>(entity);
                    return new OperationResult { Success = true, Data = updatedDto };
                }

                return new OperationResult { Success = false, Message = updateResult.Message ?? "Fallo al actualizar la entidad." };
            }
            catch (Exception ex)
            {
                return new OperationResult { Success = false, Message = $"Error: {ex.Message}" };
            }

        }
    }
}
            

