

using Microsoft.Extensions.Logging;
using SIGEBI.Application.Contracts.Service;
using SIGEBI.Application.Dtos.AuthorDTO;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.Configuration;
using SIGEBI.Domain.IRepository;

namespace SIGEBI.Application.Services
{
    public class AuthorService
        : BaseService<AuthorDTO, CreateAuthorDTO, UpdateAuthorDTO, Authors>, IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorService(IAuthorRepository authorRepository, ILogger<AuthorService> logger)
            : base(authorRepository, logger)
        {
            _authorRepository = authorRepository;
        }

        
           public async Task<OperationResult> GetGenresByAuthorAsync(int authorId)
             {
            try
            {
                if (!await _authorRepository.ExistsAsync(authorId))
                    return OperationResult.FailureResult($"Autor con ID {authorId} no encontrado.");

                var genres = await _authorRepository.GetGenresByAuthorAsync(authorId);
                return OperationResult.SuccessResult(genres);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error obteniendo géneros del autor con ID {AuthorId}", authorId);
                return OperationResult.FailureResult($"Error interno: {ex.Message}");
            }
           }

        

        public async Task<OperationResult> ExistsAsync(int authorId)
        {
            try
            {
                var exists = await _authorRepository.ExistsAsync(authorId);
                return OperationResult.SuccessResult(exists);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error verificando existencia de autor con ID {AuthorId}", authorId);
                return OperationResult.FailureResult($"Error interno: {ex.Message}");
            }
        }

        public async Task<OperationResult> CheckDuplicateAsync(string firstName, string lastName, System.DateTime? birthDate)
        {
            try
            {
                var isDuplicate = await _authorRepository.CheckDuplicateAuthorAsync(firstName, lastName, birthDate);
                return OperationResult.SuccessResult(isDuplicate);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error verificando duplicado de autor");
                return OperationResult.FailureResult($"Error interno: {ex.Message}");
            }
        }

        public async Task<OperationResult> CheckDuplicateForUpdateAsync(int id, string firstName, string lastName, System.DateTime? birthDate)
        {
            try
            {
                var isDuplicate = await _authorRepository.CheckDuplicateAuthorForUpdateAsync(id, firstName, lastName, birthDate);
                return OperationResult.SuccessResult(isDuplicate);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error verificando duplicado para actualización de autor con ID {AuthorId}", id);
                return OperationResult.FailureResult($"Error interno: {ex.Message}");
            }
        }

      

        protected override AuthorDTO MapToDto(Authors dto)
        {
            return new AuthorDTO
            {
               AuthorId  = dto.Id,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                BirthDate = dto.BirthDate,
                Nationality = dto.Nationality!,
             
            };
        }

        protected override IEnumerable<AuthorDTO> MapToDtoList(IEnumerable<Authors> entities)
        {
            return entities.Select(MapToDto);
        }

        protected override Authors MapToEntity(CreateAuthorDTO dto)
        {
            return new Authors
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                BirthDate = dto.BirthDate,
                Nationality = dto.Nationality,
                




            };
        }

        protected override Authors MapToEntity(UpdateAuthorDTO dto, Authors entity)
        {
            entity.FirstName = dto.FirstName;
            entity.LastName = dto.LastName;
            entity.BirthDate = dto.BirthDate;
            entity.Nationality = dto.Nationality;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = "System";
            return entity;
        }
    }


}




