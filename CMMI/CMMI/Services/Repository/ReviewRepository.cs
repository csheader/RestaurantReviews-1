using CMMI.DataAccess;
using CMMI.Interfaces.Repository;
using CMMI.Models;
using CMMI.Models.DTO;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace CMMI.Services.Repository
{
    public class ReviewRepository : Repository<Review>, IReviewRepository
    {
        public ReviewRepository(CMMIContext context) : base(context)
        {
        }

        public IEnumerable<Review> GetReviewsByUser(UserRequest user)
        {
            if (user == null) return null;
            return context.Reviews
                .Include(review => review.User)
                .Include(review => review.User.ContactInformation)
                .Include(review => review.Restaurant)
                .Include(review => review.Restaurant.ContactInformation)
                .Include(review => review.Restaurant.ContactInformation.Address)
                .Where(review =>
                (review.UserId == user.UserId && user.UserId != 0) ||
                (review.User.UserName == user.UserName && !string.IsNullOrEmpty(user.UserName)) ||
                (review.User.ContactInformation.Email == user.Email && !string.IsNullOrEmpty(user.Email))
                ).ToList();
        }

        public Review GetReview(long reviewId)
        {
            return context.Reviews.FirstOrDefault(review => review.ReviewId == reviewId);
        }

        public IEnumerable<Review> GetReviewsByRestaurantId(long restaurantId)
        {
            return context.Reviews.Where(review => 
                review.RestaurantId == restaurantId && restaurantId >= 0
                ).ToList();
        }

        public void Add(ReviewRequest review)
        {
            context.Reviews.Add(new Review
            {
                UserId = review.UserId,
                RestaurantId = review.RestaurantId,
                Comment = review.Comment,
                Score = review.Score,
                RatingDateTime = review.RatingDateTime
            });
        }

        public void Remove(long Id)
        {
            var reviewToDelete = context.Reviews.FirstOrDefault(review => review.ReviewId == Id);
            if (reviewToDelete != null)
            {
                context.Reviews.Attach(reviewToDelete);
                context.Reviews.Remove(reviewToDelete);
            }
        }
    }
}