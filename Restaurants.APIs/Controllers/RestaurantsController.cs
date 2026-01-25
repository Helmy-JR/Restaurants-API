using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.DeleteRestaurant;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Application.Restaurants.Queries.GetAllRestaurants;
using Restaurants.Application.Restaurants.Queries.GetRestaurantByID;


namespace Restaurants.APIs.Controllers
{
    [ApiController]
    [Route("api/restaurants")]
    public class RestaurantsController (IMediator mediator)
        : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var restaurants = await _mediator.Send(new GetAllRestaurantsQuery());
            return Ok(restaurants);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var restaurant = await _mediator.Send(new GetRestaurantByIdQyery(id));
            if (restaurant == null)
            {
                return NotFound();
            }
            return Ok(restaurant);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteRestaurant(int id)
        {
            var isDeleted = await _mediator.Send(new DeleteRestaurantCommand(id));
            if (isDeleted)
            {
                return NoContent();
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> CreateRestaurant([FromBody] CreateRestaurantCommand command)
        {
            int id = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id }, null);
        }

        [HttpPatch("{id}")]
        public async Task<ActionResult> UpdateRestaurant([FromRoute] int  id, [FromBody] UpdateRestaurantCommand command)
        {
            command.Id= id;
            var isUpdated = await _mediator.Send(command);
            if (isUpdated)
            {
                return NoContent();
            }
            return NotFound();
        }
        

    }
}