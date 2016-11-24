using CMMI.DataAccess;
using CMMI.Interfaces;
using CMMI.Models;
using CMMI.Models.DTO;
using System.Collections.Generic;
using System.Linq;
using CMMI.Interfaces.Facade;

namespace CMMI.Services.Facade
{
    public class ReviewFacade : IReviewFacade
    {
        public IEnumerable<Review> GetByUser(UserRequest user)
        {
            using (var context = new CMMIContext())
            {
                context.Configuration.LazyLoadingEnabled = false;
                IUnitOfWork unitOfWork = new UnitOfWork(context);
                return unitOfWork.Reviews.GetReviewsByUser(user);
            }
        }

        public IEnumerable<Review> GetByRestaurantId(long restaurantId)
        {
            using (var context = new CMMIContext())
            {
                context.Configuration.LazyLoadingEnabled = false;
                IUnitOfWork unitOfWork = new UnitOfWork(context);
                return unitOfWork.Reviews.GetReviewsByRestaurantId(restaurantId).ToList();
            }
        }

        public void AddReviewForRestaurant(ReviewRequest review)
        {
            using (var context = new CMMIContext())
            {
                IUnitOfWork unitOfWork = new UnitOfWork(context);
                unitOfWork.Reviews.Add(review);
                unitOfWork.Save();
            }
        }

        public void RemoveRestaurantReview(long reviewId)
        {
            using (var context = new CMMIContext())
            {
                IUnitOfWork unitOfWork = new UnitOfWork(context);
                unitOfWork.Reviews.Remove(reviewId);
                unitOfWork.Save();
            }
        }
    }
}