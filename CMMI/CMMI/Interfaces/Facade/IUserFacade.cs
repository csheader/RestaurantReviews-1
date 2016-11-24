using CMMI.Models;

namespace CMMI.Interfaces.Facade
{
    public interface IUserFacade
    {
        void AddUser(User user);
        User GetUser(User user);
    }
}