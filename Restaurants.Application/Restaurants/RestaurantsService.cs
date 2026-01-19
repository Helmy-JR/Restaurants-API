using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Restaurants.Dtos;
using AutoMapper;

namespace Restaurants.Application.Restaurants
{
    public class RestaurantsService : IRestaurantsService
    {
        private readonly IRestaurantRepository _restaurantRepo;
        private readonly ILogger<RestaurantsService> _logger;
        private readonly IMapper _Mapper;

        public RestaurantsService(
                IRestaurantRepository restaurantRepo,
                ILogger<RestaurantsService> logger,
                IMapper mapper
            )
        {
            _restaurantRepo = restaurantRepo;
            _logger = logger;
            _Mapper = mapper;
        }
        public async Task<IEnumerable<RestaurantDto>> GetAllRestaurantsAsync()
        {
            _logger.LogInformation("Fetching all Restaurants !!!");
            var restaurants = await _restaurantRepo.GetAllAsync();
            var restaurantDtos = _Mapper.Map<IEnumerable<RestaurantDto>>(restaurants);

            return restaurantDtos;
        }

        public async Task<RestaurantDto?> GetRestaurantByIdAsync(int id)
        {
            _logger.LogInformation("Fetching Restaurant by Id: {Id} !!!", id);
            var restaurant = await _restaurantRepo.GetByIdAsync(id);
            var restaurantDto = _Mapper.Map<RestaurantDto?>(restaurant);

            return restaurantDto;
        }
    }
}