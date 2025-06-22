using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Repository;

namespace SIGEBI.Persistence.Interfaces
{
    public interface IAuthorRepository : IBaseRepository<Author, int>
    {
        Task<List<Author>> GetAuthorsByNationality(string nationality);
        Task<Author?> GetAuthorByFullName(string firstName, string lastName);
        Task<OperationResult> UpdateNationality(Author author, string newNationality);
    }
}
