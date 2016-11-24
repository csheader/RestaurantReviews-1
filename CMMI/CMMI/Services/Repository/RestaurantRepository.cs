using CMMI.DataAccess;
using CMMI.Interfaces.Repository;
using CMMI.Models;
using CMMI.Models.DTO;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace CMMI.Services.Repository
{
    public class RestaurantRepository : Repository<Restaurant>, IRestaurantRepository
    {
        public RestaurantRepository(CMMIContext context) : base(context)
        {
        }

        public IEnumerable<Restaurant> GetRestaurantsByCity(string city)
        {
            return context.Restaurants.Where(restaurant => restaurant.ContactInformation.Address.City == city)
                .Include(restaurant => restaurant.ContactInformation)
                .Include(restaurant => restaurant.ContactInformation.Address)
                .ToList();
        }

        public IEnumerable<Restaurant> GetRestaurantsByAddress(RestaurantRequest address)
        {
            if (address == null) return null;
            return context.Restaurants.Where(restaurant =>
                (restaurant.ContactInformation.Address.ZipCode == address.ZipCode) ||
                (restaurant.ContactInformation.Address.City == address.City) ||
                (restaurant.RestaurantId == address.RestaurantId))
                .Include(contact => contact.ContactInformation)
                .Include(restaurant => restaurant.ContactInformation.Address)
                .ToList();
        }

        public Restaurant GetRestaurantById(long restaurantId)
        {
            return context.Restaurants.Include(restaurant => restaurant.ContactInformation)
                .Include(restaurant => restaurant.ContactInformation.Address)
                .FirstOrDefault(restaurant => restaurant.RestaurantId == restaurantId);
        }

        public Restaurant GetRestaurantByProperties(Restaurant restaurauntToFind)
        {
            return context.Restaurants.Include(restaurant => restaurant.ContactInformation)
                .Include(restaurant => restaurant.ContactInformation.Address)
                .FirstOrDefault(restaurant => (restaurauntToFind.Name == restaurant.Name
                && restaurant.ContactInformation.Address.City == restaurauntToFind.ContactInformation.Address.City) ||
                (restaurant.Name == restaurauntToFind.Name && restaurant.ContactInformation.Address.ZipCode == restaurauntToFind.ContactInformation.Address.ZipCode));
        }

        public void Remove(long Id)
        {
            var restaurantToDelete = context.Restaurants.FirstOrDefault(restaurant => restaurant.RestaurantId == Id);
            if (restaurantToDelete != null)
            {
                context.Restaurants.Attach(restaurantToDelete);
                context.Restaurants.Remove(restaurantToDelete);
            }
        }
    }
}