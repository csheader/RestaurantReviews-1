using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Results;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CMMI.Controllers;
using CMMI.Interfaces.Facade;
using CMMI.Models;
using CMMI.Models.DTO;
using Moq;

namespace CMMI.Controllers.Tests
{
    [TestClass]
    public class ReviewControllerTests
    {
        private ReviewController controller;
        private Mock<IReviewFacade> facade;
        private Review review;

        [TestInitialize]
        public void TestSetup()
        {
            review = new Review
            {
                Comment = "comment",
                RatingDateTime = DateTime.Now,
                Restaurant = new Restaurant(),
                RestaurantId = 1,
                ReviewId = 1,
                Score = 5,
                User = new User(),
                UserId = 1
            };
            facade = new Mock<IReviewFacade>();
            facade.Setup(x => x.AddReviewForRestaurant(It.IsAny<ReviewRequest>()));
            facade.Setup(x => x.GetByRestaurantId(It.IsAny<long>())).Returns(new List<Review> { review });
            facade.Setup(x => x.GetByUser(It.IsAny<UserRequest>())).Returns(new List<Review> {review});
            facade.Setup(x => x.RemoveRestaurantReview(It.IsAny<long>()));
        }

        [TestMethod]
        public void GetByUserTest()
        {
            controller = new ReviewController(facade.Object);
            var result = controller.GetByUser(null);
            var contentResult = result as OkNegotiatedContentResult<List<Review>>;
            var listFromResponse = contentResult.Content;
            Assert.AreEqual(listFromResponse.First().Restaurant, review.Restaurant);
            Assert.AreEqual(listFromResponse.First().Comment, review.Comment);
            Assert.AreEqual(listFromResponse.First().RatingDateTime, review.RatingDateTime);
            Assert.AreEqual(listFromResponse.First().RestaurantId, review.RestaurantId);
            Assert.AreEqual(listFromResponse.First().ReviewId, review.ReviewId);
            Assert.AreEqual(listFromResponse.First().Score, review.Score);
            Assert.AreEqual(listFromResponse.First().User, review.User);
            Assert.AreEqual(listFromResponse.First().UserId, review.UserId);
        }

        [TestMethod]
        public void GetByUserWithNotFoundResponse()
        {
            facade.Setup(x => x.GetByUser(It.IsAny<UserRequest>())).Returns(new List<Review>());
            controller = new ReviewController(facade.Object);
            var result = controller.GetByUser(null);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void GetByRestaurantTest()
        {
            controller = new ReviewController(facade.Object);
            var result = controller.GetByRestaurant(0);
            var contentResult = result as OkNegotiatedContentResult<List<Review>>;
            var listFromResponse = contentResult.Content;
            Assert.AreEqual(listFromResponse.First().Restaurant, review.Restaurant);
            Assert.AreEqual(listFromResponse.First().Comment, review.Comment);
            Assert.AreEqual(listFromResponse.First().RatingDateTime, review.RatingDateTime);
            Assert.AreEqual(listFromResponse.First().RestaurantId, review.RestaurantId);
            Assert.AreEqual(listFromResponse.First().ReviewId, review.ReviewId);
            Assert.AreEqual(listFromResponse.First().Score, review.Score);
            Assert.AreEqual(listFromResponse.First().User, review.User);
            Assert.AreEqual(listFromResponse.First().UserId, review.UserId);
        }

        [TestMethod]
        public void GetByRestaurantWithNotFoundResponse()
        {
            facade.Setup(x => x.GetByRestaurantId(It.IsAny<long>())).Returns(new List<Review>());
            controller = new ReviewController(facade.Object);
            var result = controller.GetByRestaurant(0);
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public void AddRestaurantReviewTest()
        {
            controller = new ReviewController(facade.Object);
            var result = controller.Add(null);
            var contentResult = result as OkNegotiatedContentResult<string>;
            Assert.AreEqual(contentResult.Content, "The review has been added for the restaurant. ");;
        }

        [TestMethod]
        public void RemoveReviewTest()
        {
            controller = new ReviewController(facade.Object);
            var result = controller.Remove(1);
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        public void RemoveWithInvalidIdTest()
        {
            controller = new ReviewController(facade.Object);
            var result = controller.Remove(-999);
            var resultAsExpectedType = result as BadRequestErrorMessageResult;
            Assert.IsInstanceOfType(result, typeof(BadRequestErrorMessageResult));
            Assert.AreEqual(resultAsExpectedType.Message, "Invalid Request, reviewId must be a non-negative integer. ");
        }
    }
}