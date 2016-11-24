using CMMI.Interfaces.Repository;
using CMMI.Models;
using System.Collections.Generic;

namespace CMMI.Interfaces.Repository
{
    public interface IRestaurantRepository : IRepository<Restaurant>
    {
        IEnumerable<Restaurant> GetRestaurantsByAddress(Models.DTO.RestaurantRequest address);
        IEnumerable<Restaurant> GetRestaurantsByCity(string city);
        Restaurant GetRestaurantByProperties(Restaurant restaurauntToFind);
        Restaurant GetRestaurantById(long restaurantId);
        void Remove(long Id);
    }
}
