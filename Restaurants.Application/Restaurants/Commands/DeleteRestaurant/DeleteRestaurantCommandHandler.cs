using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurant
{
    public class DeleteRestaurantCommandHandler : IRequestHandler<DeleteRestaurantCommand>
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
        public async Task Handle(DeleteRestaurantCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting restaurant with ID: {RestaurantId}", request.Id);
            var restaurant = await _restaurantRepository.GetByIdAsync(request.Id);
            if (restaurant == null)
            {
                _logger.LogWarning("Restaurant with ID: {RestaurantId} not found", request.Id);
                throw new NotFoundException(nameof(Restaurant), request.Id.ToString());
            }
            
            await _restaurantRepository.DeleteAsync(restaurant);
        }
    }
}