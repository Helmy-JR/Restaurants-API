using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Microsoft.Extensions.Logging;

namespace Restaurants.Application.Restaurants
{
    public class RestaurantsService : IRestaurantsService
    {
        private readonly IRestaurantRepository _restaurantRepo;
        private readonly ILogger<RestaurantsService> _logger;

        public RestaurantsService(
                IRestaurantRepository restaurantRepo,
                ILogger<RestaurantsService> logger
            )
        {
            _restaurantRepo = restaurantRepo;
            _logger = logger;
        }
        public async Task<IEnumerable<Restaurant>> GetAllRestaurantsAsync()
        {
            _logger.LogInformation("Fetching all Restaurants !!!");
            var restaurants = await _restaurantRepo.GetAllAsync();
            return restaurants;
        }
    }
}