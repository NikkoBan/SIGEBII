using SIGEBI.Domain.Entities;
using System.Collections.Generic;

namespace SIGEBI.Persistence.Interface
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
}
