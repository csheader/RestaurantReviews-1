using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using CMMI.Interfaces.Facade;
using CMMI.Models;
using CMMI.Models.DTO;

namespace CMMI.Controllers.Tests
{
    [TestClass]
    public class RestaurantControllerTests
    {
        private Mock<IRestaurantFacade> facade;
        private RestaurantController controller;
        private Restaurant restaurant;

        [TestInitialize]
        public void TestSetup()
        {
            restaurant = new Restaurant
            {
                AddressId = 1,
                ContactId = 1,
                Name = "Name",
                ContactInformation = null,
                Cuisine = "Food"
            };
            facade = new Mock<IRestaurantFacade>();
            facade.Setup(x => x.AddRestaurant(It.IsAny<Restaurant>()));
            facade.Setup(x => x.GetAllRestaurantsByAddress(It.IsAny<RestaurantRequest>())).Returns(new List<Restaurant>{restaurant});
            facade.Setup(x => x.GetRestaurantById(It.IsAny<long>())).Returns(restaurant);
        }

        [TestMethod]
        public void AddTest()
        {
            facade.Setup(x => x.GetExistingRestaurant(It.IsAny<Restaurant>())).Returns((Restaurant)null);
            controller = new RestaurantController(facade.Object);
            var result = controller.Add(null);
            var response = result as OkNegotiatedContentResult<string>;
            Assert.IsNotNull(result);
            Assert.AreEqual(response.Content, "Restaurant created successfully in the database. ");
        }

        [TestMethod]
        public void AddWithConflictResponseTest()
        {
            facade.Setup(x => x.GetExistingRestaurant(It.IsAny<Restaurant>())).Returns(restaurant);
            controller = new RestaurantController(facade.Object);
            var result = controller.Add(restaurant);
            Assert.IsInstanceOfType(result, typeof(ConflictResult));
        }

        [TestMethod]
        public void GetByCityTest()
        {
            var restaurantList = new List<Restaurant> { restaurant };
            controller = new RestaurantController(facade.Object);
            var result = controller.Get(null);
            var response = result as OkNegotiatedContentResult<List<Restaurant>>;
            var listFromResponse = response.Content;
            Assert.AreEqual(listFromResponse.First().Name, restaurantList.First().Name);
            Assert.AreEqual(listFromResponse.First().ContactInformation, restaurantList.First().ContactInformation);
            Assert.AreEqual(listFromResponse.First().AddressId, restaurantList.First().AddressId);
            Assert.AreEqual(listFromResponse.First().ContactId, restaurantList.First().ContactId);
            Assert.AreEqual(listFromResponse.First().Cuisine, restaurantList.First().Cuisine);
        }

        [TestMethod]
        public void GetByCityWithNotFoundResponse()
        {
            facade.Setup(x => x.GetAllRestaurantsByAddress(It.IsAny<RestaurantRequest>())).Returns(new List<Restaurant>());
            controller = new RestaurantController(facade.Object);
            var result = controller.Get(null);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
    }
}