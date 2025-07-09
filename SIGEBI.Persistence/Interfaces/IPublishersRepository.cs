using SIGEBI.Domain.Entities;
using SIGEBI.Domain.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Persistence.Interfaces
{
    public interface IPublishersRepository : IBaseRepository<Publishers, int>
    {
        Task<List<Publishers>> SearchByNameAsync(string name);
        Task<Publishers?> GetByEmailAsync(string email);
        
        Task<bool> ExistsByNameOrEmailAsync(string name, string email);
        Task<bool> ExistsByNameOrEmailExceptIdAsync(string name, string email, int excludeId);

    }
}
