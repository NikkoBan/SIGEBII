using SIGEBI.Application.Dtos;
using SIGEBI.Domain.Base;

namespace SIGEBI.Application.Contracts.Service
{
    public interface IBookAuthorService : IBaseService<CreateBookAuthorDTO, UpdateBookAuthorDTO, BookAuthorDTO>
    {
        Task<OperationResult> AddBookAuthorAsync(CreateBookAuthorDTO dto);
    }
}
