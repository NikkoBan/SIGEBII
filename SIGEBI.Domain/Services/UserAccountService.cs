using SIGEBI.Domain.Entities;

namespace SIGEBI.Domain.Services
{
    public interface IUserAccountService
    {
        bool Login(string email, string password, string ipAddress, string userAgent);
        void Logout(int userId);
        User? GetUserByEmail(string email);
        IEnumerable<UserHistory> GetUserHistory(int userId);
        void Register(User user);
        void UpdateUser(User user);
        void DeleteUser(int userId);
    }

    public class UserAccountService : IUserAccountService
    {
        private readonly List<User> _users;
        private readonly List<UserHistory> _userHistories;

        public UserAccountService(List<User> users, List<UserHistory> userHistories)
        {
            _users = users;
            _userHistories = userHistories;
        }

        public bool Login(string email, string password, string ipAddress, string userAgent)
        {
            var user = _users.FirstOrDefault(u => u.InstitutionalEmail == email && u.PasswordHash == password && u.IsActive);
            var history = new UserHistory
            {
                UserId = user?.UserId ?? 0,
                EnteredEmail = email,
                AttemptDate = DateTime.Now,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                IsSuccessful = user != null,
                FailureReason = user == null ? "Invalid credentials" : null,
                ObtainedRole = user?.RoleId.ToString()
            };
            _userHistories.Add(history);
            return user != null;
        }

        public void Logout(int userId)
        {
            // Optionally, add a logout event to history or handle session logic
            /*
               var user = _users.FirstOrDefault(u => u.UserId == userId);
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
                         IpAddress = null,    // Puedes pasar la IP si la tienes
                         UserAgent = null     // Puedes pasar el UserAgent si lo tienes
               };
                            _userHistories.Add(history);
                         }
             */
        }

        public User? GetUserByEmail(string email)
        {
            return _users.FirstOrDefault(u => u.InstitutionalEmail == email);
        }

        public IEnumerable<UserHistory> GetUserHistory(int userId)
        {
            return _userHistories.Where(h => h.UserId == userId);
        }

        public void Register(User user)
        {
            user.UserId = _users.Count > 0 ? _users.Max(u => u.UserId) + 1 : 1;
            user.RegistrationDate = DateTime.Now;
            user.IsActive = true;
            _users.Add(user);
        }

        public void UpdateUser(User user)
        {
            var existing = _users.FirstOrDefault(u => u.UserId == user.UserId);
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
            }
        }

        public void DeleteUser(int userId)
        {
            var user = _users.FirstOrDefault(u => u.UserId == userId);
            if (user != null)
            {
                user.IsDeleted = true;
                user.DeletedAt = DateTime.Now;
                // Optionally, remove from _users list
            }
        }
    }
}