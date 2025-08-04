using SIGEBI.Application.DTOsAplication.LoanStatusDTOs;
using SIGEBI.Application.Interfaces;
using SIGEBI.Application.Interfaces.MappersInterfaces;
using SIGEBI.Persistence.Base;
using SIGEBI.Persistence.Interface;

namespace SIGEBI.Application.Services
{
    public class LoanStatusService : ILoanStatusService
    {
        private readonly ILoanStatusRepository _repository;
        private readonly IMapperLoanStatusDto _mapper;

        public LoanStatusService(ILoanStatusRepository repository, IMapperLoanStatusDto mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OperationResult<List<LoanStatusDisplayDTO>>> GetAllAsync()
        {
            var result = await _repository.GetAllAsync();
            if (!result.Success)
                return OperationResult<List<LoanStatusDisplayDTO>>.Fail(result.Errors, result.Message ?? "Error inesperado");

            var dtoList = result.Data?.ConvertAll(_mapper.ToDisplayDto) ?? new List<LoanStatusDisplayDTO>();
            return OperationResult<List<LoanStatusDisplayDTO>>.Ok(dtoList);
        }

        public async Task<OperationResult<LoanStatusDisplayDTO>> GetByIdAsync(int id)
        {
            var result = await _repository.GetByIdAsync(id);
            if (!result.Success)
                return OperationResult<LoanStatusDisplayDTO>.Fail(result.Message ?? "Error inesperado");

            if (result.Data == null)
                return OperationResult<LoanStatusDisplayDTO>.Fail("LoanStatus no encontrado");

            return OperationResult<LoanStatusDisplayDTO>.Ok(_mapper.ToDisplayDto(result.Data));
        }

        public async Task<OperationResult<LoanStatusDisplayDTO>> CreateAsync(LoanStatusCreationDTO dto)
        {
            var entity = _mapper.FromCreationDto(dto);
            var result = await _repository.AddAsync(entity);

            if (!result.Success)
                return OperationResult<LoanStatusDisplayDTO>.Fail(result.Errors, result.Message ?? "Error inesperado");

            if (result.Data == null)
                return OperationResult<LoanStatusDisplayDTO>.Fail("No se pudo crear LoanStatus");

            return OperationResult<LoanStatusDisplayDTO>.Ok(_mapper.ToDisplayDto(result.Data));
        }

        public async Task<OperationResult<LoanStatusDisplayDTO>> UpdateAsync(int id, LoanStatusUpdateDTO dto)
        {
            var getResult = await _repository.GetByIdAsync(id);
            if (!getResult.Success || getResult.Data == null)
                return OperationResult<LoanStatusDisplayDTO>.Fail("LoanStatus no encontrado");

            _mapper.UpdateEntity(getResult.Data, dto);

            var updateResult = await _repository.UpdateAsync(getResult.Data);
            if (!updateResult.Success)
                return OperationResult<LoanStatusDisplayDTO>.Fail(updateResult.Errors, updateResult.Message ?? "Error inesperado");

            return OperationResult<LoanStatusDisplayDTO>.Ok(_mapper.ToDisplayDto(getResult.Data));
        }

        public async Task<OperationResult<bool>> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
