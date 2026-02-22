using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.DeleteAllDishesForRestaunt
{
    public class DeleteAllDishesForRestaurantCommandHandler : IRequestHandler<DeleteAllDishesForRestaurantCommand>
    {
        private readonly ILogger<DeleteAllDishesForRestaurantCommand> _logger;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IDishesRepository _dishesRepository;
        private readonly IRestaurantAuthorizationService _restaurantAuthorizationService;

        public DeleteAllDishesForRestaurantCommandHandler(
            ILogger<DeleteAllDishesForRestaurantCommand> logger,
            IRestaurantRepository restaurantRepository,
            IDishesRepository dishesRepository,
            IRestaurantAuthorizationService restaurantAuthorizationService
        )
        {
            _logger = logger;
            _restaurantRepository = restaurantRepository;
            _dishesRepository = dishesRepository;
            _restaurantAuthorizationService = restaurantAuthorizationService;
        }
        public async Task Handle(DeleteAllDishesForRestaurantCommand request, CancellationToken cancellationToken)
        {
            _logger.LogWarning("Deleting all dishes for restaurant with id: {RestaurantId}", request.RestaurantId);
            var restaurant = await _restaurantRepository.GetByIdAsync(request.RestaurantId);
            if (restaurant == null) throw new NotFoundException(nameof(restaurant), request.RestaurantId.ToString());
            
            if(! _restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Delete))
            {
                _logger.LogWarning("Unauthorized attempt to delete dishes for restaurant with ID: {RestaurantId}", restaurant.Id);
                throw new ForbidException();
            }

            await _dishesRepository.DeleteAllDishes(restaurant.Dishes);
        }
    }
}