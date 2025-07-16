using SIGEBI.Application.Dtos.AuthorDTO;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.Configuration;




namespace SIGEBI.Application.Contracts.Service
{
    public interface IAuthorService : IBaseService<AuthorDTO, CreateAuthorDTO, UpdateAuthorDTO>
    {
        Task<OperationResult> GetGenresByAuthorAsync(int authorId);
        Task<OperationResult> ExistsAsync(int authorId);
        Task<OperationResult> CheckDuplicateAsync(string firstName, string lastName, DateTime? birthDate);
        Task<OperationResult> CheckDuplicateForUpdateAsync(int id, string firstName, string lastName, DateTime? birthDate);
    }
}
