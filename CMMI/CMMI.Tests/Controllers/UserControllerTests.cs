using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CMMI.Controllers;
using CMMI.Interfaces.Facade;
using CMMI.Models;
using Moq;

namespace CMMI.Controllers.Tests
{
    [TestClass]
    public class UserControllerTests
    {
        private Mock<IUserFacade> facade;
        private UserController controller;
        private User user;

        [TestInitialize]
        public void TestSetup()
        {
            user = new User
            {
                ContactInformation = null, 
                FirstName = "firstName",
                LastName = "lastName",
                UserId = 0,
                UserName = "PicardWasBetterThanKirk"
            };
            facade = new Mock<IUserFacade>();
            facade.Setup(x => x.AddUser(It.IsAny<User>()));
        }

        [TestMethod]
        public void AddNewUserTest()
        {
            facade.Setup(x => x.GetUser(It.IsAny<User>())).Returns(user);
            controller = new UserController(facade.Object);
            var result = controller.AddNewUser(user);
            Assert.IsInstanceOfType(result, typeof(ConflictResult));
        }

        [TestMethod]
        public void AddNewUserInvalidUsernameReturnsBadRequest()
        {
            user.UserName = null;
            controller = new UserController(facade.Object);
            var result = controller.AddNewUser(user);
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
            var contentResult = result as BadRequestErrorMessageResult;
            Assert.AreEqual(contentResult.Message, "A userName must be supplied and the supplied UserId must be empty or 0. ");
        }

        [TestMethod]
        public void AddNewUserInvalidUserIdReturnsBadRequest()
        {
            user.UserId = 999;
            controller = new UserController(facade.Object);
            var result = controller.AddNewUser(user);
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
            var contentResult = result as BadRequestErrorMessageResult;
            Assert.AreEqual(contentResult.Message, "A userName must be supplied and the supplied UserId must be empty or 0. ");
        }

        [TestMethod]
        public void AddNewUserTestWithOkResult()
        {
            facade.Setup(x => x.GetUser(It.IsAny<User>())).Returns((User) null);
            controller = new UserController(facade.Object);
            var result = controller.AddNewUser(user);
            Assert.IsInstanceOfType(result, typeof(OkNegotiatedContentResult<string>));
            var contentResult = result as OkNegotiatedContentResult<string>;
            Assert.AreEqual(contentResult.Content, "User successfully created. ");
        }
    }
}