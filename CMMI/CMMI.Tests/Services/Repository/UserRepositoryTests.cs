using Microsoft.VisualStudio.TestTools.UnitTesting;
using CMMI.Services.Repository;
using Moq;
using CMMI.Models;
using System;
using CMMI.DataAccess;
using System.Collections.Generic;
using System.Linq;

namespace CMMI.Services.Repository.Tests
{
    [TestClass]
    public class UserRepositoryTests
    {
        private UserRepository repository;
        private User currentUser;
        private User user;


        [TestInitialize]
        public void TestSetup()
        {
            user = new User
            {
                ContactInformation = new Contact
                {
                    Address = new Address
                    {
                        Address1 = "address1",
                        Address2 = "address2",
                        Address3 = "address3",
                        City = Guid.NewGuid().ToString(),
                        Country = "USA",
                        State = "PA",
                        ZipCode = Guid.NewGuid().ToString()
                    },
                    Email = Guid.NewGuid().ToString()
                },
                FirstName = "FirstName",
                LastName = "LastName",
                UserName = Guid.NewGuid().ToString()
            };
            AddUserRecordsForTest();
        }

        private void AddUserRecordsForTest()
        {
            using (var context = new CMMIContext())
            {
                repository = new UserRepository(context);
                repository.Add(user);
                context.SaveChanges();
                currentUser = repository.GetUser(user);
            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
            using (var context = new CMMIContext())
            {
                repository = new UserRepository(context);
                repository.Remove(user.UserId);
                repository.Save();
            }
        }

        [TestMethod]
        public void GetUsersTest()
        {
            List<User> result;
            using (var context = new CMMIContext())
            {
                repository = new UserRepository(context);
                result = repository.GetUsers().ToList();
            }
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetUserByUserObjectTest()
        {
            using (var context = new CMMIContext())
            {
                repository = new UserRepository(context);
                currentUser = repository.GetUser(user);
            }
            Assert.AreEqual(currentUser.FirstName, user.FirstName);
            Assert.AreEqual(currentUser.LastName, user.LastName);
            Assert.AreEqual(currentUser.UserName, user.UserName);
            Assert.AreEqual(currentUser.ContactInformation.Address.Address1, user.ContactInformation.Address.Address1);
            Assert.AreEqual(currentUser.ContactInformation.Address.Address2, user.ContactInformation.Address.Address2);
            Assert.AreEqual(currentUser.ContactInformation.Address.Address3, user.ContactInformation.Address.Address3);
            Assert.AreEqual(currentUser.ContactInformation.Address.City, user.ContactInformation.Address.City);
            Assert.AreEqual(currentUser.ContactInformation.Address.Country, user.ContactInformation.Address.Country);
            Assert.AreEqual(currentUser.ContactInformation.Address.State, user.ContactInformation.Address.State);
            Assert.AreEqual(currentUser.ContactInformation.Address.ZipCode, user.ContactInformation.Address.ZipCode);
            Assert.AreEqual(currentUser.ContactInformation.Email, user.ContactInformation.Email);
            Assert.AreEqual(currentUser.ContactInformation.Phone, user.ContactInformation.Phone);
        }

        [TestMethod]
        public void GetUserByIdTest()
        {
            using (var context = new CMMIContext())
            {
                repository = new UserRepository(context);
                currentUser = repository.GetUser(currentUser.UserId);
            }
            Assert.AreEqual(currentUser.FirstName, user.FirstName);
            Assert.AreEqual(currentUser.LastName, user.LastName);
            Assert.AreEqual(currentUser.UserName, user.UserName);
            Assert.AreEqual(currentUser.ContactInformation.Address.Address1, user.ContactInformation.Address.Address1);
            Assert.AreEqual(currentUser.ContactInformation.Address.Address2, user.ContactInformation.Address.Address2);
            Assert.AreEqual(currentUser.ContactInformation.Address.Address3, user.ContactInformation.Address.Address3);
            Assert.AreEqual(currentUser.ContactInformation.Address.City, user.ContactInformation.Address.City);
            Assert.AreEqual(currentUser.ContactInformation.Address.Country, user.ContactInformation.Address.Country);
            Assert.AreEqual(currentUser.ContactInformation.Address.State, user.ContactInformation.Address.State);
            Assert.AreEqual(currentUser.ContactInformation.Address.ZipCode, user.ContactInformation.Address.ZipCode);
            Assert.AreEqual(currentUser.ContactInformation.Email, user.ContactInformation.Email);
            Assert.AreEqual(currentUser.ContactInformation.Phone, user.ContactInformation.Phone);
        }
    }
}