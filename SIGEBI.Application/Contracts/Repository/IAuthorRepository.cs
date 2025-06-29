using SIGEBI.Domain.Entities.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Application.Contracts.Repository
{
    public interface IAuthorRepository : IBaseRepository<Authors>
    {


        Task<bool> CheckDuplicateAuthorAsync(string firstName, string lastName, DateTime? birthDate);
        Task<bool> CheckDuplicateAuthorForUpdateAsync(int id, string firstName, string lastName, DateTime? birthDate);
    }
}
