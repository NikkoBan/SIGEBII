using SIGEBI.Domain.Entities;
using System;

namespace SIGEBI.Application.Validation
{
    public static class UserHistoryValidation
    {
        public static bool IsValid(UserHistory history)
        {
            if (history == null)
            {
                return false;
            }

            if (history.UserId <= 0)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(history.EnteredEmail))
            {
                return false;
            }

            if (history.AttemptDate == DateTime.MinValue)
            {
                return false;
            }

            if (history.IsSuccessful && !string.IsNullOrWhiteSpace(history.FailureReason))
            {
                return false;
            }

            return true;
        }
    }
}