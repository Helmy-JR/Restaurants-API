using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Dishes.Commands.DeleteAllDishesForRestaunt
{
    public class DeleteAllDishesForRestaurantCommandHandler : IRequestHandler<DeleteAllDishesForRestaurantCommand>
    {
        private readonly ILogger<DeleteAllDishesForRestaurantCommand> _logger;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IDishesRepository _dishesRepository;

        public DeleteAllDishesForRestaurantCommandHandler(
            ILogger<DeleteAllDishesForRestaurantCommand> logger,
            IRestaurantRepository restaurantRepository,
            IDishesRepository dishesRepository
        )
        {
            _logger = logger;
            _restaurantRepository = restaurantRepository;
            _dishesRepository = dishesRepository;
        }
        public async Task Handle(DeleteAllDishesForRestaurantCommand request, CancellationToken cancellationToken)
        {
            _logger.LogWarning("Deleting all dishes for restaurant with id: {RestaurantId}", request.RestaurantId);
            var restaurant = await _restaurantRepository.GetByIdAsync(request.RestaurantId);
            if (restaurant == null) throw new NotFoundException(nameof(restaurant), request.RestaurantId.ToString());
            
            await _dishesRepository.DeleteAllDishes(restaurant.Dishes);
        }
    }
}