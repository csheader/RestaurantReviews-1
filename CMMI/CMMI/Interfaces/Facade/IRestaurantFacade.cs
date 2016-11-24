using System.Collections.Generic;
using CMMI.Models;
using CMMI.Models.DTO;

namespace CMMI.Interfaces.Facade
{
    public interface IRestaurantFacade
    {
        IEnumerable<Restaurant> GetAllRestaurantsByAddress(RestaurantRequest address);
        Restaurant GetRestaurantById(long restaurantId);
        void AddRestaurant(Restaurant restaurant);
        Restaurant GetExistingRestaurant(Restaurant restaurant);
    }
}