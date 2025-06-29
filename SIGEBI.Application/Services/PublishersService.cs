using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Interfaces;


namespace SIGEBI.Application.Services
{
    public class PublishersService : IPublishersService
    {
        private readonly IPublishersRepository _repository;

        public PublishersService(IPublishersRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Publishers>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Publishers?> GetByIdAsync(int id)
        {
            return await _repository.GetEntityByIdAsync(id);
        }

        public async Task<OperationResult> CreateAsync(Publishers publisher)
        {
            publisher.CreatedBy = "system"; // O el usuario autenticado
            publisher.CreatedAt = DateTime.UtcNow;
            publisher.UpdatedBy = null;
            publisher.UpdatedAt = publisher.CreatedAt;
            publisher.IsDeleted = false;
            publisher.DeletedBy = null;
            return await _repository.SaveEntityAsync(publisher);
        }

        public async Task<OperationResult> UpdateAsync(Publishers publisher)
        {
            publisher.UpdatedBy = "system"; // O el usuario autenticado
            publisher.UpdatedAt = DateTime.UtcNow;
            return await _repository.UpdateEntityAsync(publisher);
        }

        public async Task<OperationResult> DeleteAsync(int id)
        {
            var publisher = await _repository.GetEntityByIdAsync(id);
            if (publisher == null)
                return OperationResult.Fail("Editorial no encontrada.");

            return await _repository.RemoveEntityAsync(publisher);
        }

        public async Task<List<Publishers>> SearchByNameAsync(string name)
        {
            return await _repository.SearchByNameAsync(name);
        }

        public async Task<Publishers?> GetByEmailAsync(string email)
        {
            return await _repository.GetByEmailAsync(email);
        }
    }
}
