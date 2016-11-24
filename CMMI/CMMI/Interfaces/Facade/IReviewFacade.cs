using System.Collections.Generic;
using CMMI.Models;
using CMMI.Models.DTO;

namespace CMMI.Interfaces.Facade
{
    public interface IReviewFacade
    {
        IEnumerable<Review> GetByUser(UserRequest user);
        IEnumerable<Review> GetByRestaurantId(long restaurantId);
        void AddReviewForRestaurant(ReviewRequest review);
        void RemoveRestaurantReview(long reviewId);
    }
}