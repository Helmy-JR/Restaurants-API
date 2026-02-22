using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.DeleteRestaurant
{
    public class DeleteRestaurantCommandHandler : IRequestHandler<DeleteRestaurantCommand>
    {
        private readonly ILogger _logger;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IRestaurantAuthorizationService _restaurantAuthorizationService;

        public DeleteRestaurantCommandHandler(
            ILogger<DeleteRestaurantCommand> logger,
            IRestaurantRepository restaurantRepository,
            IRestaurantAuthorizationService restaurantAuthorizationService
        )
        {
            _logger = logger;
            _restaurantRepository = restaurantRepository;
            _restaurantAuthorizationService = restaurantAuthorizationService;
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

            if(! _restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Delete))
            {
                _logger.LogWarning("Unauthorized attempt to delete restaurant with ID: {RestaurantId}", request.Id);
                throw new ForbidException();
            }
            
            await _restaurantRepository.DeleteAsync(restaurant);
        }
    }
}