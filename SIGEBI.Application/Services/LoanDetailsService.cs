using SIGEBI.Application.DTOsAplication.LoanDetailsDTOs;
using SIGEBI.Application.Interfaces;
using SIGEBI.Application.Interfaces.Mappers;
using SIGEBI.Persistence.Interface;
using SIGEBI.Persistence.Base;

namespace SIGEBI.Application.Services
{
    public class LoanDetailsService : ILoanDetailsService
    {
        private readonly ILoanDetailsRepository _repository;
        private readonly IMapperLoanDetailsDto _mapper;

        public LoanDetailsService(ILoanDetailsRepository repository, IMapperLoanDetailsDto mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OperationResult<List<LoanDetailsDisplayDTO>>> GetAllAsync()
        {
            var result = await _repository.GetAllAsync();
            if (!result.Success)
                return OperationResult<List<LoanDetailsDisplayDTO>>.Fail(result.Errors, result.Message);

            var dtoList = result.Data?.ConvertAll(_mapper.ToDisplayDto) ?? new List<LoanDetailsDisplayDTO>();
            return OperationResult<List<LoanDetailsDisplayDTO>>.Ok(dtoList);
        }

        public async Task<OperationResult<LoanDetailsDisplayDTO>> GetByIdAsync(int id)
        {
            var result = await _repository.GetByIdAsync(id);

            if (!result.Success)
                return OperationResult<LoanDetailsDisplayDTO>.Fail(result.Message ?? "Error desconocido al obtener el LoanDetail.");

            var entity = result.Data;
            if (entity is null)
                return OperationResult<LoanDetailsDisplayDTO>.Fail("LoanDetail no encontrado");

            var dto = _mapper.ToDisplayDto(entity);
            return OperationResult<LoanDetailsDisplayDTO>.Ok(dto);
        }

        public async Task<OperationResult<LoanDetailsDisplayDTO>> CreateAsync(LoanDetailsCreationDTO dto)
        {
            var entity = _mapper.FromCreationDto(dto);
            var result = await _repository.AddAsync(entity);

            if (!result.Success)
                return OperationResult<LoanDetailsDisplayDTO>.Fail(result.Errors, result.Message);

            var dtoResult = _mapper.ToDisplayDto(result.Data!);
            return OperationResult<LoanDetailsDisplayDTO>.Ok(dtoResult);
        }

        public async Task<OperationResult<LoanDetailsDisplayDTO>> UpdateAsync(int id, LoanDetailsUpdateDTO dto)
        {
            var getResult = await _repository.GetByIdAsync(id);

            if (!getResult.Success || getResult.Data is null)
                return OperationResult<LoanDetailsDisplayDTO>.Fail("LoanDetail no encontrado");

            var entity = getResult.Data;
            _mapper.UpdateEntity(entity, dto);

            var updateResult = await _repository.UpdateAsync(entity);
            if (!updateResult.Success)
                return OperationResult<LoanDetailsDisplayDTO>.Fail(updateResult.Errors, updateResult.Message);

            var dtoResult = _mapper.ToDisplayDto(entity);
            return OperationResult<LoanDetailsDisplayDTO>.Ok(dtoResult);
        }

        public async Task<OperationResult<bool>> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
