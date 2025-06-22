using SIGEBI.Persistence.Interface;
using System.Collections.Generic;
using System.Linq;

namespace SIGEBI.Persistence.Repositori
{
    public class UserRepository : IUserRepository
    {
        private readonly List<User> _users;

        public UserRepository(List<User> users)
        {
            _users = users;
        }

        public User? GetByEmail(string email) => _users.FirstOrDefault(u => u.InstitutionalEmail == email);
        public User? GetById(int userId) => _users.FirstOrDefault(u => u.UserId == userId);
        public IEnumerable<User> GetAll() => _users;
        public void Add(User user) => _users.Add(user);
        public void Update(User user)
        {
            var idx = _users.FindIndex(u => u.UserId == user.UserId);
            if (idx >= 0) _users[idx] = user;
        }
    }
}