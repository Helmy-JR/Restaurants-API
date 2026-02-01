using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.UpdateRestaurant
{
    public class UpdateRestaurantCommandHandler : IRequestHandler<UpdateRestaurantCommand>
    {
        private readonly ILogger _logger;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IMapper _mapper;

        public UpdateRestaurantCommandHandler(
            ILogger<UpdateRestaurantCommand> logger,
            IRestaurantRepository restaurantRepository,
            IMapper mapper
        )
        {
            _logger = logger;
            _restaurantRepository = restaurantRepository;
            _mapper = mapper;
        }
        public async Task Handle(UpdateRestaurantCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating restaurant with ID: {RestaurantId} with {@updatedRestaurant}", request.Id, request);
            var restaurant = await _restaurantRepository.GetByIdAsync(request.Id);
            if (restaurant == null)
            {
                throw new NotFoundException(nameof(Restaurant), request.Id.ToString());
            }
            _mapper.Map(request, restaurant);

            await _restaurantRepository.SaveChanges();
        }
    }
}