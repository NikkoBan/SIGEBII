using SIGEBI.Application.DTOsAplication.LoanDTOs;
using SIGEBI.Application.Interfaces;
using SIGEBI.Application.Interfaces.Mappers;
using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Base;
using SIGEBI.Persistence.Interface;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SIGEBI.Application.Services
{
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IMapperLoanDto _mapper;

        public LoanService(ILoanRepository loanRepository, IMapperLoanDto mapper)
        {
            _loanRepository = loanRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult<List<LoanDisplayDTO>>> GetAllAsync()
        {
            var result = await _loanRepository.GetAllAsync();

            if (!result.Success)
                return OperationResult<List<LoanDisplayDTO>>.Fail(result.Errors, result.Message);

            var dtos = new List<LoanDisplayDTO>();
            foreach (var loan in result.Data ?? new List<Loan>())
            {
                dtos.Add(_mapper.ToDisplayDto(loan));
            }

            return OperationResult<List<LoanDisplayDTO>>.Ok(dtos);
        }

        public async Task<OperationResult<LoanDisplayDTO>> GetByIdAsync(int id)
        {
            var result = await _loanRepository.GetByIdAsync(id);

            if (!result.Success)
                return OperationResult<LoanDisplayDTO>.Fail(result.Errors, result.Message);

            if (result.Data == null)
                return OperationResult<LoanDisplayDTO>.Fail("No se encontró el préstamo.");

            var dto = _mapper.ToDisplayDto(result.Data);

            return OperationResult<LoanDisplayDTO>.Ok(dto);
        }

        public async Task<OperationResult<LoanDisplayDTO>> CreateAsync(LoanCreationDTO dto)
        {
            var loan = _mapper.FromCreationDto(dto);
            var result = await _loanRepository.AddAsync(loan);

            if (!result.Success || result.Data == null)
                return OperationResult<LoanDisplayDTO>.Fail(result.Errors, result.Message);

            var createdDto = _mapper.ToDisplayDto(result.Data);

            return OperationResult<LoanDisplayDTO>.Ok(createdDto);
        }

        public async Task<OperationResult<LoanDisplayDTO>> UpdateAsync(int id, LoanUpdateDTO dto)
        {
            var existingResult = await _loanRepository.GetByIdAsync(id);

            if (!existingResult.Success)
                return OperationResult<LoanDisplayDTO>.Fail(existingResult.Errors, existingResult.Message);

            if (existingResult.Data == null)
                return OperationResult<LoanDisplayDTO>.Fail("No se encontró el préstamo para actualizar.");

            _mapper.UpdateEntity(existingResult.Data, dto);

            var updateResult = await _loanRepository.UpdateAsync(existingResult.Data);

            if (!updateResult.Success)
                return OperationResult<LoanDisplayDTO>.Fail(updateResult.Errors, updateResult.Message);

            var updatedDto = _mapper.ToDisplayDto(existingResult.Data);

            return OperationResult<LoanDisplayDTO>.Ok(updatedDto);
        }

        public async Task<OperationResult<bool>> DeleteAsync(int id)
        {
            var deleteResult = await _loanRepository.DeleteAsync(id);

            if (!deleteResult.Success)
                return OperationResult<bool>.Fail(deleteResult.Errors, deleteResult.Message);

            return OperationResult<bool>.Ok(true, "Préstamo eliminado correctamente.");
        }
    }
}
