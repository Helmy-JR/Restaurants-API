using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurant
{
    public class DeleteRestaurantCommandHandler : IRequestHandler<DeleteRestaurantCommand,bool>
    {
        private readonly ILogger _logger;
        private readonly IRestaurantRepository _restaurantRepository;

        public DeleteRestaurantCommandHandler(
            ILogger<DeleteRestaurantCommand> logger,
            IRestaurantRepository restaurantRepository
        )
        {
            _logger = logger;
            _restaurantRepository = restaurantRepository;
        }
        public async Task<bool> Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting restaurant with ID: {RestaurantId}", request.Id);
            var restaurant = await _restaurantRepository.GetByIdAsync(request.Id);
            if (restaurant == null)
            {
                _logger.LogWarning("Restaurant with ID: {RestaurantId} not found", request.Id);
                return false;
            }
            ;
            await _restaurantRepository.DeleteAsync(restaurant);
            return true;
        }
    }
}