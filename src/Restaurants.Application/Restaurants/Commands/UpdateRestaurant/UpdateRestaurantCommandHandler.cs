using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant
{
    public class UpdateRestaurantCommandHandler : IRequestHandler<UpdateRestaurantCommand>
    {
        private readonly ILogger _logger;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IMapper _mapper;
        private readonly IRestaurantAuthorizationService _restaurantAuthorizationService;

        public UpdateRestaurantCommandHandler(
            ILogger<UpdateRestaurantCommand> logger,
            IRestaurantRepository restaurantRepository,
            IMapper mapper,
            IRestaurantAuthorizationService restaurantAuthorizationService
        )
        {
            _logger = logger;
            _restaurantRepository = restaurantRepository;
            _mapper = mapper;
            _restaurantAuthorizationService = restaurantAuthorizationService;
        }
        public async Task Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating restaurant with ID: {RestaurantId} with {@updatedRestaurant}", request.Id, request);
            var restaurant = await _restaurantRepository.GetByIdAsync(request.Id);
            if (restaurant == null)
            {
                throw new NotFoundException(nameof(Restaurant), request.Id.ToString());
            }

            if(! _restaurantAuthorizationService.Authorize(restaurant, ResourceOperation.Update))
            {
                _logger.LogWarning("Unauthorized attempt to update restaurant with ID: {RestaurantId}", request.Id);
                throw new ForbidException();
            }

            _mapper.Map(request, restaurant);

            await _restaurantRepository.SaveChanges();
        }
    }
}