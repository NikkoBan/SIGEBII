using SIGEBI.Application.Contracts.Interface;
using SIGEBI.Domain.Entities.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace SIGEBI.Application.Contracts.Repository
{


    public interface IPublisherRepository : IBaseRepository<Publisher>
    {
        Task<bool> CheckDuplicatePublisherNameAsync(string publisherName, int? excludePublisherId = null);

    }
}
