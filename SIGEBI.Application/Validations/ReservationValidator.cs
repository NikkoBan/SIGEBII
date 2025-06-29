using SIGEBI.Domain.Entities.circulation;
using SIGEBI.Domain.Base;
using SIGEBI.Domain.Entities.core;
using SIGEBI.Domain.Entities.library;

namespace SIGEBI.Application.Validations
{
    public class ReservationValidator
    {
        public static OperationResult ValidateReservation(Reservation entity)
        {
            if (entity == null)
            {
                return OperationResult.Failure("Reservation entity cannot be null.");
            }

            if (entity.BookId <= 0)
            {
                return OperationResult.Failure("BookId is required and must be greater than 0.");
            }

            if (entity.UserId <= 0)
            {
                return OperationResult.Failure("UserId is required and must be greater than 0.");
            }

            if (entity.ReservationDate < DateTime.Today)
            {
                return OperationResult.Failure("Reservation date cannot be in the past.");
            }

            if (entity.ExpirationDate <= entity.ReservationDate)
            {
                return OperationResult.Failure("Expiration date must be after the reservation date.");
            }

            return OperationResult.Success("Validation passed.");
        }

        public static OperationResult ValidateReservationId(int id)
        {
            if (id <= 0)
            {
                return OperationResult.Failure("Reservation ID must be greater than 0.");
            }

            return OperationResult.Success("ID validation passed.");
        }
    }
}

