using System;
using System.Linq;
using CMMI.DataAccess;
using CMMI.Models;
using CMMI.Models.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CMMI.Services.Repository;
using Moq;

namespace CMMI.Services.Repository.Tests
{
    [TestClass]
    public class ReviewRepositoryTests
    {
        private ReviewRepository repository;
        private RestaurantRepository restaurantRepository;
        private UserRepository userRepository;
        private ReviewRequest reviewRequest;
        private RestaurantRequest restaurantRequest;
        private Review review;
        private User user;
        private Restaurant currentRestaurant;
        private Review currentReview;
        private User currentUser;
        private Restaurant restaurantToAdd;

        [TestInitialize]
        public void TestSetup()
        {
            restaurantToAdd = new Restaurant
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
            
            AddRestaurantRecordsForTest();
            AddUserRecordsForTest();
            review = new Review
            {
                Comment = "Test comment",
                RatingDateTime = DateTime.UtcNow,
                RestaurantId = currentRestaurant.RestaurantId,
                UserId = currentUser.UserId,
                Score = 3
            };
            AddReviewRecordsForTest();
            
        }

        private void AddUserRecordsForTest()
        {
            using (var context = new CMMIContext())
            {
                userRepository = new UserRepository(context);
                userRepository.Add(user);
                context.SaveChanges();
                currentUser = userRepository.GetUser(user);
            }
        }

        private void AddRestaurantRecordsForTest()
        {
            using (var context = new CMMIContext())
            {
                restaurantRepository = new RestaurantRepository(context);
                restaurantRepository.Add(restaurantToAdd);
                context.SaveChanges();
                currentRestaurant = restaurantRepository.GetRestaurantByProperties(restaurantToAdd);
            }
        }

        private void AddReviewRecordsForTest()
        {
            using (var context = new CMMIContext())
            {
                repository = new ReviewRepository(context);
                repository.Add(review);
                context.SaveChanges();
                currentReview = repository.GetReviewsByRestaurantId(currentRestaurant.RestaurantId).FirstOrDefault();
            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
            using (var context = new CMMIContext())
            {
                repository = new ReviewRepository(context);
                repository.Remove(currentReview.ReviewId);
                context.SaveChanges();
                userRepository = new UserRepository(context);
                userRepository.Remove(user.UserId);
                context.SaveChanges();
                restaurantRepository = new RestaurantRepository(context);
                restaurantRepository.Remove(currentRestaurant.RestaurantId);
                context.SaveChanges();
            }
        }

        [TestMethod]
        public void GetReviewsByUserTest()
        {
            using (var context = new CMMIContext())
            {
                repository = new ReviewRepository(context);
                currentReview = repository.GetReviewsByUser(new UserRequest
                {
                    Email = user.ContactInformation.Email,
                    UserId = user.UserId,
                    UserName = user.UserName
                }).FirstOrDefault();
            }
            Assert.IsNotNull(currentReview);
            Assert.AreEqual(currentReview.Comment, review.Comment);
            Assert.AreEqual(currentReview.RatingDateTime.Date, review.RatingDateTime.Date);
            Assert.AreEqual(currentReview.RestaurantId, review.RestaurantId);
            Assert.AreEqual(currentReview.Score, review.Score);
            Assert.AreEqual(currentReview.UserId, review.UserId);
        }

        [TestMethod]
        public void GetReviewTest()
        {
            using (var context = new CMMIContext())
            {
                repository = new ReviewRepository(context);
                currentReview = repository.GetReview(currentReview.ReviewId);
            }
            Assert.IsNotNull(currentReview);
            Assert.AreEqual(currentReview.Comment, review.Comment);
            Assert.AreEqual(currentReview.RatingDateTime.Date, review.RatingDateTime.Date);
            Assert.AreEqual(currentReview.RestaurantId, review.RestaurantId);
            Assert.AreEqual(currentReview.Score, review.Score);
            Assert.AreEqual(currentReview.UserId, review.UserId);
        }

        [TestMethod]
        public void GetReviewsByRestaurantTest()
        {
            using (var context = new CMMIContext())
            {
                repository = new ReviewRepository(context);
                currentReview = repository.GetReviewsByRestaurantId(currentReview.RestaurantId).FirstOrDefault();
            }
            Assert.IsNotNull(currentReview);
            Assert.AreEqual(currentReview.Comment, review.Comment);
            Assert.AreEqual(currentReview.RestaurantId, review.RestaurantId);
            Assert.AreEqual(currentReview.Score, review.Score);
            Assert.AreEqual(currentReview.UserId, review.UserId);
        }

        [TestMethod]
        public void AddTest()
        {
            using (var context = new CMMIContext())
            {
                repository = new ReviewRepository(context);
                repository.Add(review);
                repository.Save();
                var result = repository.GetReviewsByRestaurantId(currentReview.RestaurantId).ToList();
                Assert.AreEqual(result.Count,2);
                repository.Remove(result.First().ReviewId);
                currentReview = result.Last();
            }
        }
    }
}