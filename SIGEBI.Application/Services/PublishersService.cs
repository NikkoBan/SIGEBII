using SIGEBI.Application.DTOs.PublishersDTOs;
using SIGEBI.Application.Interfaces;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities;
using SIGEBI.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<List<PublisherDto>> GetAllAsync()
        {
            var publishers = await _repository.GetAllAsync();
            return publishers.Select(p => p.ToDto()).ToList();
        }

        public async Task<PublisherDto?> GetByIdAsync(int id)
        {
            var publisher = await _repository.GetEntityByIdAsync(id);
            return publisher?.ToDto();
        }

        public async Task<OperationResult> CreateAsync(CreationPublisherDto publisherDto)
        {
            
            if (await _repository.ExistsByNameOrEmailAsync(publisherDto.PublisherName, publisherDto.Email))
                return OperationResult.Fail("Ya existe una editorial con ese nombre o correo.");

            var publisher = publisherDto.ToEntity();
            publisher.CreatedBy = "system";
            publisher.CreatedAt = DateTime.UtcNow;

            return await _repository.SaveEntityAsync(publisher);
        }

        public async Task<OperationResult> UpdateAsync(UpdatePublisherDto publisherDto)
        {
            var publisher = await _repository.GetEntityByIdAsync(publisherDto.Id);
            if (publisher == null)
                return OperationResult.Fail("Editorial no encontrada.");

           
            if (await _repository.ExistsByNameOrEmailExceptIdAsync(publisherDto.PublisherName, publisherDto.Email, publisherDto.Id))
                return OperationResult.Fail("Ya existe otra editorial con ese nombre o correo.");

            publisherDto.UpdateEntity(publisher);
            publisher.UpdatedBy = "system";
            publisher.UpdatedAt = DateTime.UtcNow;

            return await _repository.UpdateEntityAsync(publisher);
        }

        public async Task<OperationResult> DeleteAsync(RemovePublisherDto publisherDto)
        {
            var publisher = await _repository.GetEntityByIdAsync(publisherDto.Id);
            if (publisher == null)
                return OperationResult.Fail("Editorial no encontrada.");

            publisher.MarkAsDeleted(publisherDto.DeletedBy, publisherDto.Reason);
            return await _repository.UpdateEntityAsync(publisher);
        }

        public async Task<List<PublisherDto>> SearchByNameAsync(string name)
        {
            var publishers = await _repository.SearchByNameAsync(name);
            return publishers.Select(p => p.ToDto()).ToList();
        }

        public async Task<PublisherDto?> GetByEmailAsync(string email)
        {
            var publisher = await _repository.GetByEmailAsync(email);
            return publisher?.ToDto();
        }
    }
}
