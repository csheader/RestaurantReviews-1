using CMMI.DataAccess;
using CMMI.Interfaces;
using CMMI.Interfaces.Facade;
using CMMI.Models;

namespace CMMI.Services.Facade
{
    public class UserFacade : IUserFacade
    {
        public void AddUser(User user)
        {
            using (var context = new CMMIContext())
            {
                IUnitOfWork unitOfWork = new UnitOfWork(context);
                unitOfWork.Users.Add(user);
                unitOfWork.Save();
            }
        }

        public User GetUser(User user)
        {
            using (var context = new CMMIContext())
            {
                context.Configuration.LazyLoadingEnabled = false;
                IUnitOfWork unitOfWork = new UnitOfWork(context);
                return unitOfWork.Users.GetUser(user);
            }
        }
    }
}