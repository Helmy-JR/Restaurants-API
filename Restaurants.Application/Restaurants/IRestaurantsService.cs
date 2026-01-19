
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Restaurants
{
    public interface IRestaurantsService
    {
        public Task<IEnumerable<RestaurantDto>> GetAllRestaurantsAsync();

        public Task<RestaurantDto?> GetRestaurantByIdAsync(int id);
    }
}