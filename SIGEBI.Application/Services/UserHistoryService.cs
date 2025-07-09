using AutoMapper;
using SIGEBI.Application.DTOsAplication.UserHistoryDTOs;
using SIGEBI.Application.Interfaces;
using SIGEBI.Application.Validation;
using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Base;
using SIGEBI.Persistence.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace SIGEBI.Application.Services
{
    public class UserHistoryService : IUserHistoryService
    {
        private readonly IUserHistoryRepository _userHistoryRepository;
        private readonly IMapper _mapper;

        public UserHistoryService(IUserHistoryRepository userHistoryRepository, IMapper mapper)
        {
            _userHistoryRepository = userHistoryRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult<UserHistoryDisplayDto>> GetUserHistoryByIdAsync(int id)
        {
            var history = await _userHistoryRepository.GetByIdAsync(id);
            if (history == null)
            {
                return OperationResult<UserHistoryDisplayDto>.Fail("Entrada de historial no encontrada.", null, 404);
            }
            var historyDto = _mapper.Map<UserHistoryDisplayDto>(history);
            return OperationResult<UserHistoryDisplayDto>.Ok(historyDto);
        }

        public async Task<OperationResult<IEnumerable<UserHistoryDisplayDto>>> GetAllUserHistoryAsync()
        {
            var historyEntries = await _userHistoryRepository.GetAllAsync();
            var historyDtos = _mapper.Map<IEnumerable<UserHistoryDisplayDto>>(historyEntries);
            return OperationResult<IEnumerable<UserHistoryDisplayDto>>.Ok(historyDtos);
        }

        public async Task<OperationResult<IEnumerable<UserHistoryDisplayDto>>> GetUserHistoryByUserIdAsync(int userId)
        {
            var historyEntries = await _userHistoryRepository.GetByUserIdAsync(userId);
            if (historyEntries == null || !historyEntries.Any())
            {
                return OperationResult<IEnumerable<UserHistoryDisplayDto>>.Fail("No se encontró historial para el usuario especificado.", null, 404);
            }
            var historyDtos = _mapper.Map<IEnumerable<UserHistoryDisplayDto>>(historyEntries);
            return OperationResult<IEnumerable<UserHistoryDisplayDto>>.Ok(historyDtos);
        }

        public async Task<OperationResult<UserHistoryDisplayDto>> CreateUserHistoryAsync(UserHistoryCreationDto historyDto)
        {
            var history = _mapper.Map<UserHistory>(historyDto);

            if (!UserHistoryValidation.IsValid(history))
            {
                return OperationResult<UserHistoryDisplayDto>.Fail("Datos de historial inválidos.", null, 400);
            }

            history.AttemptDate = DateTime.Now;
            history.CreatedAt = DateTime.Now;
            history.CreatedBy = "API_System";

            var result = await _userHistoryRepository.AddAsync(history);

            if (!result.Success)
            {
                return OperationResult<UserHistoryDisplayDto>.Fail(result.Message ?? "Error al crear la entrada de historial.", null, result.StatusCode);
            }

            var createdHistoryDto = _mapper.Map<UserHistoryDisplayDto>(history);
            return OperationResult<UserHistoryDisplayDto>.Ok(createdHistoryDto, "Entrada de historial creada exitosamente.");
        }

        public async Task<OperationResult> DeleteUserHistoryAsync(int id)
        {
            var historyToDelete = await _userHistoryRepository.GetByIdAsync(id);
            if (historyToDelete == null)
            {
                return OperationResult.Fail("Entrada de historial no encontrada para eliminar.", null, 404);
            }

            // Opcional: Implementar borrado lógico si la tabla UserHistory tiene IsDeleted, etc.
            // historyToDelete.IsDeleted = true;
            // historyToDelete.DeletedAt = DateTime.Now;
            // historyToDelete.DeletedBy = "API_System";
            // var result = await _userHistoryRepository.UpdateAsync(historyToDelete);

            var result = await _userHistoryRepository.RemoveAsync(id);

            if (!result.Success)
            {
                return OperationResult.Fail(result.Message ?? "Error al eliminar la entrada de historial.", null, result.StatusCode);
            }
            return OperationResult.Ok("Entrada de historial eliminada exitosamente.");
        }
    }
}