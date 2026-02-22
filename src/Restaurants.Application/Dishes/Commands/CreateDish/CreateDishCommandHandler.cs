using MediatR;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Constants;

namespace Restaurants.Application.Dishes.Commands.CreateDish
{
    public class CreateDishCommandHandler : IRequestHandler<CreateDishCommand, int>
    {
        private readonly ILogger<CreateDishCommandHandler> _logger;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IDishesRepository _dishesRepository;
        private readonly IMapper _mapper;
        private readonly IRestaurantAuthorizationService _restaurantAuthorizationService;

        public CreateDishCommandHandler(
            ILogger<CreateDishCommandHandler> logger,
            IRestaurantRepository restaurantRepository,
            IDishesRepository dishesRepository,
            IMapper mapper,
            IRestaurantAuthorizationService restaurantAuthorizationService
        )
        {
            _logger = logger;
            _restaurantRepository = restaurantRepository;
            _dishesRepository = dishesRepository;
            _mapper = mapper;
            _restaurantAuthorizationService = restaurantAuthorizationService;
        }


        public async Task<int> Handle(CreateDishCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating new dish {@DishRequest}", request);
            var restaurant = await _restaurantRepository.GetByIdAsync(request.RestaurantId);
            if(restaurant == null)
            {
                throw new NotFoundException(nameof(restaurant),request.RestaurantId.ToString());
            }

            if(! _restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Create))
            {
                _logger.LogWarning("Unauthorized attempt to create dish for restaurant with ID: {RestaurantId}", restaurant.Id);
                throw new ForbidException();
            }
            var dish = _mapper.Map<Dish>(request);
            return await _dishesRepository.Create(dish);
        }
    }
}