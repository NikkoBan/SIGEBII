using SIGEBI.Application.DTOs;
using SIGEBI.Application.Validations;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.circulation;

namespace SIGEBI.Application.Contracts.Services
{
    public interface IReservationStatusService
    {
        Task<OperationResult> GetAllStatusesAsync();
        Task<OperationResult> GetStatusByIdAsync(int id);

    }
}
