using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Application.Services
{
    public interface IPublishersService
    {
        Task<List<Publishers>> GetAllAsync();
        Task<Publishers?> GetByIdAsync(int id);
        Task<OperationResult> CreateAsync(Publishers publisher);
        Task<OperationResult> UpdateAsync(Publishers publisher);
        Task<OperationResult> DeleteAsync(int id);
        Task<List<Publishers>> SearchByNameAsync(string name);
        Task<Publishers?> GetByEmailAsync(string email);


    }
}
