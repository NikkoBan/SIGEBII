using SIGEBI.Domain.Entities;
using System.Text.RegularExpressions;
using System;

namespace SIGEBI.Application.Validation
{
    public static class UserValidation
    {
        public static bool IsValid(User user)
        {
            if (user == null)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(user.FullName) || user.FullName.Length < 3 || user.FullName.Length > 200)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(user.InstitutionalEmail) || !IsValidEmail(user.InstitutionalEmail))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(user.PasswordHash) || user.PasswordHash.Length < 6)
            {
                return false;
            }

            if (user.RoleId <= 0)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(user.CreatedBy) || user.CreatedBy.Length < 2)
            {
                return false;
            }

            return true;
        }

        private static bool IsValidEmail(string email)
        {
            string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailPattern);
        }
    }
}