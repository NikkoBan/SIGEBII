using SIGEBI.Application.Contracts.Repository;
using SIGEBI.Application.Contracts.Service;
using SIGEBI.Application.Dtos;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using AutoMapper;

namespace SIGEBI.Application.Services
{
    public class AuthorService : BaseService<Authors, CreateAuthorDTO, UpdateAuthorDTO, AuthorDTO>, IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly ILogger<AuthorService> _logger;

        public AuthorService(
            IAuthorRepository authorRepository,
            IMapper mapper,
            ILogger<AuthorService> logger)
            : base(authorRepository, mapper)
        {
            _authorRepository = authorRepository;
            _logger = logger;
        }

      
        public async Task<bool> CheckDuplicateAuthorAsync(string firstName, string lastName, DateTime? birthDate)
        {
            return await _authorRepository.CheckDuplicateAuthorAsync(firstName, lastName, birthDate);
        }

        public async Task<bool> CheckDuplicateAuthorForUpdateAsync(int id, string firstName, string lastName, DateTime? birthDate)
        {
            return await _authorRepository.CheckDuplicateAuthorForUpdateAsync(id, firstName, lastName, birthDate);
        }

      
        public override async Task<OperationResult> CreateAsync(CreateAuthorDTO dto)
        {
            if (string.IsNullOrWhiteSpace(dto.FirstName) || string.IsNullOrWhiteSpace(dto.LastName))
            {
                _logger.LogWarning("Intento de crear autor fallido: Nombre y apellido son obligatorios.");
                return new OperationResult { Success = false, Message = "Nombre y apellido del autor son obligatorios." };
            }

            bool isDuplicate = await _authorRepository.CheckDuplicateAuthorAsync(dto.FirstName, dto.LastName, dto.BirthDate);
            if (isDuplicate)
            {
                _logger.LogWarning($"Intento de crear autor fallido: Autor duplicado encontrado: {dto.FirstName} {dto.LastName}.");
                return new OperationResult { Success = false, Message = "Ya existe un autor con el mismo nombre, apellido y fecha de nacimiento." };
            }

            var result = await base.CreateAsync(dto);

            if (result.Success)
            {
                result.Message = "Autor creado exitosamente.";
            }
            else
            {
                _logger.LogError($"Error al crear autor: {result.Message}");
            }

            return result;
        }

        public override async Task<OperationResult> UpdateAsync(int id, UpdateAuthorDTO dto)
        {
            if (id <= 0 || dto.AuthorId != id)
            {
                return new OperationResult { Success = false, Message = "ID de autor inválido o no coincide." };
            }

            if (string.IsNullOrWhiteSpace(dto.FirstName) || string.IsNullOrWhiteSpace(dto.LastName))
            {
                return new OperationResult { Success = false, Message = "Nombre y apellido son obligatorios para actualizar." };
            }

            bool isDuplicate = await _authorRepository.CheckDuplicateAuthorForUpdateAsync(id, dto.FirstName, dto.LastName, dto.BirthDate);
            if (isDuplicate)
            {
                return new OperationResult { Success = false, Message = "Ya existe otro autor con los mismos datos." };
            }

            return await base.UpdateAsync(id, dto);
        }
    }
}

