using SIGEBI.Domain.Entities;
using SIGEBI.Persistence.Interface;
using System.Collections.Generic;
using System.Linq;

namespace SIGEBI.Persistence.Repositori
{
    public class UserAccountService : IUserAccountService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserHistoryRepository _userHistoryRepository;

        public UserAccountService(IUserRepository userRepository, IUserHistoryRepository userHistoryRepository)
        {
            _userRepository = userRepository;
            _userHistoryRepository = userHistoryRepository;
        }

        public bool Login(string email, string password, string ipAddress, string userAgent)
        {
            var user = _userRepository.GetByEmail(email);
            bool isValid = user != null && user.PasswordHash == password && user.IsActive;

            var history = new UserHistory
            {
                UserId = user?.UserId ?? 0,
                EnteredEmail = email,
                AttemptDate = DateTime.Now,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                IsSuccessful = isValid,
                FailureReason = isValid ? null : "Invalid credentials",
                ObtainedRole = user?.RoleId.ToString()
            };
            _userHistoryRepository.Add(history);

            return isValid;
        }

        public void Logout(int userId)
        {
            var user = _userRepository.GetById(userId);
            if (user != null)
            {
                var history = new UserHistory
                {
                    UserId = userId,
                    EnteredEmail = user.InstitutionalEmail,
                    AttemptDate = DateTime.Now,
                    IsSuccessful = true,
                    FailureReason = null,
                    ObtainedRole = user.RoleId.ToString(),
                    IpAddress = null,
                    UserAgent = null
                };
                _userHistoryRepository.Add(history);
            }
        }

        public User? GetUserByEmail(string email) => _userRepository.GetByEmail(email);

        public IEnumerable<UserHistory> GetUserHistory(int userId) => _userHistoryRepository.GetByUserId(userId);

        public void Register(User user)
        {
            var allUsers = _userRepository.GetAll();
            user.UserId = allUsers.Any() ? allUsers.Max(u => u.UserId) + 1 : 1;
            user.RegistrationDate = DateTime.Now;
            user.IsActive = true;
            _userRepository.Add(user);
        }

        public void UpdateUser(User user)
        {
            var existing = _userRepository.GetById(user.UserId);
            if (existing != null)
            {
                existing.FullName = user.FullName;
                existing.InstitutionalEmail = user.InstitutionalEmail;
                existing.PasswordHash = user.PasswordHash;
                existing.InstitutionalIdentifier = user.InstitutionalIdentifier;
                existing.RoleId = user.RoleId;
                existing.IsActive = user.IsActive;
                existing.UpdatedAt = DateTime.Now;
                existing.UpdatedBy = user.UpdatedBy;
                _userRepository.Update(existing);
            }
        }

        public void DeleteUser(int userId)
        {
            var user = _userRepository.GetById(userId);
            if (user != null)
            {
                user.IsDeleted = true;
                user.DeletedAt = DateTime.Now;
                _userRepository.Update(user);
            }
        }
    }
}
