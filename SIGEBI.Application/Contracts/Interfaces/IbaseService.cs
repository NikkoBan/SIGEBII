using SIGEBI.Domain.Base;
using System;
using System.Collections.Generic;


namespace SIGEBI.Application.Contracts.Service
{
    public interface IBaseService<TDto, TCreateDto, TUpdateDto>
    {
        Task<OperationResult> GetByIdAsync(int id);
        Task<OperationResult> GetAllAsync();
        Task<OperationResult> CreateAsync(TCreateDto dto);
        Task<OperationResult> UpdateAsync(int id, TUpdateDto dto);
        Task<OperationResult> DeleteAsync(int id);
       
    }
}

