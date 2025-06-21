using SIGEBI.Application.Contracts;
using SIGEBI.Application.Contracts.Repositories.Reservations;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.circulation;
using System.Linq.Expressions;

namespace SIGEBI.Persistence.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        public Task<OperationResult> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }
        public Task<OperationResult> GetAllAsync(Expression<Func<Reservation, bool>> filter)
        {
            throw new NotImplementedException();
        }
        public Task<bool> ExistsAsync(Expression<Func<Reservation, bool>> filter)
        {
            throw new NotImplementedException();
        }
        public Task<OperationResult> AddAsync(Reservation entity)
        {
            throw new NotImplementedException();
        }
        public Task<OperationResult> UpdateAsync(Reservation entity)
        {
            throw new NotImplementedException();
        }
        public Task<OperationResult> DeleteAsync(Reservation entity)
        {
            throw new NotImplementedException();
        }       
    }
}
