using System.Linq;
using System.Linq.Expressions;
using SIGEBI.Domain.Entities.circulation;
using SIGEBI.Persistence.Repositories;

namespace SIGEBI.Persistence.Test
{
    [TestClass]
    public class ReservationRepositoryTests
    {
        private ReservationRepository _repository =null!;

        [TestInitialize]
        public void Setup()
        {
            _repository = new ReservationRepository();
        }

        [TestMethod]
        public async Task GetByIdAsync_ThrowsNotImplementedException()
        {
            await Assert.ThrowsExceptionAsync<NotImplementedException>(() => _repository.GetByIdAsync(1));
        }

        [TestMethod]
        public async Task GetAllAsync_ThrowsNotImplementedException()
        {
            Expression<Func<Reservation, bool>> filter = r => r.Id > 0;
            await Assert.ThrowsExceptionAsync<NotImplementedException>(() => _repository.GetAllAsync(filter));
        }

        [TestMethod]
        public async Task ExistsAsync_ThrowsNotImplementedException()
        {
            Expression<Func<Reservation, bool>> filter = r => r.Id > 0;
            await Assert.ThrowsExceptionAsync<NotImplementedException>(() => _repository.ExistsAsync(filter));
        }

        [TestMethod]
        public async Task AddAsync_ThrowsNotImplementedException()
        {
            var reservation = new Reservation();
            await Assert.ThrowsExceptionAsync<NotImplementedException>(() => _repository.AddAsync(reservation));
        }

        [TestMethod]
        public async Task UpdateAsync_ThrowsNotImplementedException()
        {
            var reservation = new Reservation();
            await Assert.ThrowsExceptionAsync<NotImplementedException>(() => _repository.UpdateAsync(reservation));
        }

        [TestMethod]
        public async Task DeleteAsync_ThrowsNotImplementedException()
        {
            var reservation = new Reservation();
            await Assert.ThrowsExceptionAsync<NotImplementedException>(() => _repository.DeleteAsync(reservation));
        }
    }
}
