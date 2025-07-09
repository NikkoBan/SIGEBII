using SIGEBI.Application.DTOs;
using SIGEBI.Application.DTOs.PublishersDTOs;
using SIGEBI.Domain.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIGEBI.Application.Interfaces
{
    public interface IPublishersService
    {
        Task<List<PublisherDto>> GetAllAsync();
        Task<PublisherDto?> GetByIdAsync(int id);
        Task<OperationResult> CreateAsync(CreationPublisherDto publisherDto);
        Task<OperationResult> UpdateAsync(UpdatePublisherDto publisherDto);
        Task<OperationResult> DeleteAsync(RemovePublisherDto publisherDto);
        Task<List<PublisherDto>> SearchByNameAsync(string name);
        Task<PublisherDto?> GetByEmailAsync(string email);

    }


}

