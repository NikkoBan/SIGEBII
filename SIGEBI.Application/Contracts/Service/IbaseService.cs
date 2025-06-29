using SIGEBI.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Application.Contracts.Service
{
    public interface IBaseService<TCreateDto, TUpdateDto, TReadDto>
        where TCreateDto : class
        where TUpdateDto : class
        where TReadDto : class
    {
        Task<OperationResult> CreateAsync(TCreateDto dto);
        Task<OperationResult> GetByIdAsync(int id);
        Task<OperationResult> GetAllAsync();
        Task<OperationResult> UpdateAsync(int id, TUpdateDto dto);
        Task<OperationResult> DeleteAsync(int id);
    }
}
