using SIGEBI.Application.Contracts.Repository;
using SIGEBI.Application.Dtos;

using SIGEBI.Domain.Entities.Configuration;




namespace SIGEBI.Application.Contracts.Service
{
    public interface IAuthorService : IBaseService<CreateAuthorDTO, UpdateAuthorDTO, AuthorDTO>
    {

        Task<bool> CheckDuplicateAuthorAsync(string firstName, string lastName, DateTime? birthDate);
        Task<bool> CheckDuplicateAuthorForUpdateAsync(int id, string firstName, string lastName, DateTime? birthDate);
      

    }
}
