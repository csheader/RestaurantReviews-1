using CMMI.Models;
using System.Collections.Generic;

namespace CMMI.Interfaces.Repository
{
    public interface IUserRepository : IRepository<User>
    {
        IEnumerable<User> GetUsers();
        User GetUser(long userId);
        User GetUser(User user);
        void Remove(long id);
    }
}
