using SIGEBI.Application.DTOsAplication.LoanHistoryDTOs;
using SIGEBI.Application.Interfaces;
using SIGEBI.Application.Interfaces.MappersInterfaces;
using SIGEBI.Persistence.Base;
using SIGEBI.Persistence.Interface;

namespace SIGEBI.Application.Services
{
    public class LoanHistoryService : ILoanHistoryService
    {
        private readonly ILoanHistoryRepository _loanHistoryRepository;
        private readonly IMapperLoanHistoryDto _mapper;

        public LoanHistoryService(ILoanHistoryRepository loanHistoryRepository, IMapperLoanHistoryDto mapper)
        {
            _loanHistoryRepository = loanHistoryRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult<LoanHistoryDisplayDTO>> CreateAsync(LoanHistoryCreationDTO dto)
        {
            var entity = _mapper.FromCreationDto(dto);
            var result = await _loanHistoryRepository.AddAsync(entity);
            if (!result.Success)
                return OperationResult<LoanHistoryDisplayDTO>.Fail(result.Message ?? "Error desconocido");

            if (result.Data == null)
                return OperationResult<LoanHistoryDisplayDTO>.Fail("No se pudo crear el LoanHistory");

            var displayDto = _mapper.ToDisplayDto(result.Data);
            return OperationResult<LoanHistoryDisplayDTO>.Ok(displayDto);
        }

        public async Task<OperationResult<LoanHistoryDisplayDTO>> GetByIdAsync(int id)
        {
            var result = await _loanHistoryRepository.GetByIdAsync(id);
            if (!result.Success)
                return OperationResult<LoanHistoryDisplayDTO>.Fail(result.Message ?? "Error desconocido");

            if (result.Data == null)
                return OperationResult<LoanHistoryDisplayDTO>.Fail("LoanHistory no encontrado");

            var dto = _mapper.ToDisplayDto(result.Data);
            return OperationResult<LoanHistoryDisplayDTO>.Ok(dto);
        }

        public async Task<OperationResult<List<LoanHistoryDisplayDTO>>> GetAllAsync()
        {
            var result = await _loanHistoryRepository.GetAllAsync();
            if (!result.Success)
                return OperationResult<List<LoanHistoryDisplayDTO>>.Fail(result.Message ?? "Error desconocido");

            if (result.Data == null)
                return OperationResult<List<LoanHistoryDisplayDTO>>.Fail("No se encontraron LoanHistories");

            var dtoList = new List<LoanHistoryDisplayDTO>();
            foreach (var entity in result.Data)
                dtoList.Add(_mapper.ToDisplayDto(entity));

            return OperationResult<List<LoanHistoryDisplayDTO>>.Ok(dtoList);
        }

        public async Task<OperationResult<LoanHistoryDisplayDTO>> UpdateAsync(int id, LoanHistoryUpdateDTO dto)
        {
            var getResult = await _loanHistoryRepository.GetByIdAsync(id);
            if (!getResult.Success)
                return OperationResult<LoanHistoryDisplayDTO>.Fail(getResult.Message ?? "Error desconocido");

            if (getResult.Data == null)
                return OperationResult<LoanHistoryDisplayDTO>.Fail("LoanHistory no encontrado");

            var entity = getResult.Data;
            _mapper.UpdateEntity(entity, dto);

            var updateResult = await _loanHistoryRepository.UpdateAsync(entity);
            if (!updateResult.Success)
                return OperationResult<LoanHistoryDisplayDTO>.Fail(updateResult.Message ?? "Error desconocido");

            var updatedDto = _mapper.ToDisplayDto(entity);
            return OperationResult<LoanHistoryDisplayDTO>.Ok(updatedDto);
        }

        public async Task<OperationResult<bool>> DeleteAsync(int id)
        {
            var result = await _loanHistoryRepository.DeleteAsync(id);
            if (!result.Success)
                return OperationResult<bool>.Fail(result.Message ?? "Error desconocido");

            return OperationResult<bool>.Ok(true);
        }
    }
}
