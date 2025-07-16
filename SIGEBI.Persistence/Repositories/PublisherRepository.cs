

//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;

//using SIGEBI.Domain.Base;
//using SIGEBI.Domain.Entities.Configuration;
//using SIGEBI.Persistence.Base.SIGEBI.Persistence.Repositories;
//using SIGEBI.Persistence.Context;
//using SIGEBI.Persistence.IRepository;

//namespace SIGEBI.Persistence.Repositories
//{

//    public class PublisherRepository : BaseRepositoryEf<Publisher>, PublisherRepository
//    {
//        public PublisherRepository(SIGEBIContext context, ILogger<PublisherRepository> logger)
//             : base(context, logger) { }

//        public async Task<bool> CheckDuplicatePublisherNameAsync(string publisherName, int? excludePublisherId = null)
//        {
//            try
//            {
//                if (excludePublisherId.HasValue)
//                {

//                    return await _dbSet.AnyAsync(p => p.PublisherName == publisherName && p.Id != excludePublisherId.Value);
//                }

//                return await _dbSet.AnyAsync(p => p.PublisherName == publisherName);
//            }
//            catch (System.Exception ex)
//            {
//                _logger.LogError(ex, $"Error checking for duplicate publisher name: {publisherName}.");

//                return false;
//            }

//        }
//        public override async Task<OperationResult> GetByIdAsync(int id)
//        {
//            try
//            {
//                var publisher = await _dbSet.FirstOrDefaultAsync(p => p.Id == id); 

//                if (publisher == null)
//                {
//                    return new OperationResult { Success = false, Message = $"Publisher with ID {id} not found." };
//                }
//                return new OperationResult { Success = true, Data = publisher };
//            }
//            catch (System.Exception ex)
//            {
//                _logger.LogError(ex, $"Error getting publisher with ID {id}.");
//                return new OperationResult { Success = false, Message = $"Error getting publisher: {ex.Message}" };
//            }
//        }
//    }
//}

