using System.Linq;
using CMMI.DataAccess;
using CMMI.Models;
using CMMI.Models.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CMMI.Services.Repository.Tests
{
    [TestClass]
    public class RestaurantRepositoryTests
    {
        private RestaurantRepository repository;
        private Restaurant restaurantToAdd;
        private RestaurantRequest restaurantRequest;
        private long currentRowNumber;

        [TestInitialize]
        public void TestSetup()
        {
            restaurantToAdd = new Restaurant
            {
                ContactInformation = new Contact { Address = new Address
                {
                    Address1 = "address1",
                    Address2 = "address2",
                    Address3 = "address3",
                    City = "city",
                    Country = "USA",
                    State = "PA",
                    ZipCode = "zipcode"
                },
                Email = "Captain@Kirk.com",
                Phone = "412-867-5309"
                },
                Cuisine = "cuisine",
                Name = "Milliways"
            };
            restaurantRequest = new RestaurantRequest
            {
                City = restaurantToAdd.ContactInformation.Address.City,
                ZipCode = restaurantToAdd.ContactInformation.Address.ZipCode
            };
            AddTestRecordToDatabase();
        }

        private void AddTestRecordToDatabase()
        {
            using (var context = new CMMIContext())
            {
                repository = new RestaurantRepository(context);
                repository.Add(restaurantToAdd);
                context.SaveChanges();
                var restaurant = repository.GetRestaurantByProperties(restaurantToAdd);
                currentRowNumber = restaurant.RestaurantId;
            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
            using (var context = new CMMIContext())
            {
                repository = new RestaurantRepository(context);
                var restaurant = repository.GetRestaurantById(currentRowNumber);
                repository.Remove(restaurant.RestaurantId);
                context.SaveChanges();
            }
        }

        [TestMethod]
        public void GetRestaurantsByCityTest()
        {
            using (var context = new CMMIContext())
            {
                context.Configuration.LazyLoadingEnabled = false;
                repository = new RestaurantRepository(context);
                var result = repository.GetRestaurantsByCity("city").ToList();
                var singleResult = result.FirstOrDefault(x => x.ContactInformation.Address.City == "city");
                Assert.IsNotNull(singleResult);
            }
        }

        [TestMethod]
        public void GetRestaurantsByAddressTest()
        {
            using (var context = new CMMIContext())
            {
                context.Configuration.LazyLoadingEnabled = false;
                repository = new RestaurantRepository(context);
                var result = repository.GetRestaurantsByAddress(restaurantRequest);
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void GetRestaurantByIdTest()
        {
            using (var context = new CMMIContext())
            {
                context.Configuration.LazyLoadingEnabled = false;
                repository = new RestaurantRepository(context);
                var result = repository.GetRestaurantById(currentRowNumber);
                Assert.IsNotNull(result);
            }
        }

        [TestMethod]
        public void GetRestaurantByPropertiesTest()
        {
            using (var context = new CMMIContext())
            {
                context.Configuration.LazyLoadingEnabled = false;
                repository = new RestaurantRepository(context);
                var restaurant = repository.GetRestaurantById(currentRowNumber);
                var result = repository.GetRestaurantByProperties(restaurant);
                Assert.IsNotNull(result);
            }
        }
    }
}