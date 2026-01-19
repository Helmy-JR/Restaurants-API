using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants;

namespace Restaurants.APIs.Controllers
{
    [ApiController]
    [Route("api/restaurants")]
    public class RestaurantsController (IRestaurantsService restaurantsService)
        : ControllerBase
    {
        private readonly IRestaurantsService _restaurantsService = restaurantsService;

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var restaurants = await _restaurantsService.GetAllRestaurantsAsync();
            return Ok(restaurants);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var restaurant = await _restaurantsService.GetRestaurantByIdAsync(id);
            if (restaurant == null)
            {
                return NotFound();
            }
            return Ok(restaurant);
        }

    }
}