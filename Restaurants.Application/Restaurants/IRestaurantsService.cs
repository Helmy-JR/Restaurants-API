
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Restaurants
{
    public interface IRestaurantsService
    {
        public Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync();
    }
}