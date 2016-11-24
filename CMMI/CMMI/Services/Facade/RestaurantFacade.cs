using CMMI.DataAccess;
using CMMI.Interfaces;
using CMMI.Models;
using System.Collections.Generic;
using CMMI.Interfaces.Facade;
using CMMI.Models.DTO;

namespace CMMI.Services.Facade
{
    public class RestaurantFacade : IRestaurantFacade
    {

        public IEnumerable<Restaurant> GetAllRestaurantsByAddress(RestaurantRequest address)
        {
            using (var context = new CMMIContext())
            {
                context.Configuration.LazyLoadingEnabled = false;
                IUnitOfWork unitOfWork = new UnitOfWork(context);
                return unitOfWork.Restaurants.GetRestaurantsByAddress(address);
            }
        }

        public Restaurant GetRestaurantById(long restaurantId)
        {
            using (var context = new CMMIContext())
            {
                context.Configuration.LazyLoadingEnabled = false;
                IUnitOfWork unitOfWork = new UnitOfWork(context);
                return unitOfWork.Restaurants.GetRestaurantById(restaurantId);
            }
        }

        public void AddRestaurant(Restaurant restaurant)
        {
            using (var context = new CMMIContext())
            {
                IUnitOfWork unitOfWork = new UnitOfWork(context);
                unitOfWork.Restaurants.Add(restaurant);
                unitOfWork.Save();
            }
        }

        public Restaurant GetExistingRestaurant(Restaurant restaurant)
        {
            using (var context = new CMMIContext())
            {
                context.Configuration.LazyLoadingEnabled = false;
                IUnitOfWork unitOfWork = new UnitOfWork(context);
                return unitOfWork.Restaurants.GetRestaurantByProperties(restaurant);
            }
        }
    }
}
