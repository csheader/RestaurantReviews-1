using CMMI.Models;
using CMMI.Models.DTO;
using System.Collections.Generic;

namespace CMMI.Interfaces.Repository
{
    public interface IReviewRepository : IRepository<Review>
    {
        IEnumerable<Review> GetReviewsByUser(UserRequest user);
        IEnumerable<Review> GetReviewsByRestaurantId(long restaurantId);
        Review GetReview(long reviewId);
        void Add(ReviewRequest review);
        void Remove(long Id);
    }
}
