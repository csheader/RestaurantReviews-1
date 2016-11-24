using CMMI.DataAccess;
using CMMI.Interfaces.Repository;
using CMMI.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace CMMI.Services.Repository
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(CMMIContext context) : base(context)
        {
        }

        public IEnumerable<User> GetUsers()
        {
            return context.Users
                .Include(user => user.ContactInformation)
                .Include(user => user.ContactInformation.Address)
                .ToList();
        }

        public User GetUser(long userId)
        {
            return context.Users.Include(x => x.ContactInformation)
                .Include(user => user.ContactInformation.Address)
                .FirstOrDefault(user => user.UserId == userId);
        }

        public User GetUser(User user)
        {
            return context.Users
                .Include(userInfo => userInfo.ContactInformation)
                .Include(userInfo => userInfo.ContactInformation.Address)
                .FirstOrDefault(storedUser => user.UserName == storedUser.UserName);
        }

        public void Remove(long Id)
        {
            var userToDelete = context.Users.FirstOrDefault(user => user.UserId == Id);
            if (userToDelete != null)
            {
                context.Users.Attach(userToDelete);
                context.Users.Remove(userToDelete);
            }
        }
    }
}