using MediatR;
using Restaurants.Domain.Entities;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Repositories;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant
{
    public class CreateRestaurantCommandHandler
     : IRequestHandler<CreateRestaurantCommand, int>
    {
        private readonly ILogger<CreateRestaurantCommandHandler> _logger ;
        private readonly IRestaurantRepository _restaurantRepo ;
        private readonly IMapper _mapper ;

        public CreateRestaurantCommandHandler(
            ILogger<CreateRestaurantCommandHandler> logger,
            IRestaurantRepository restaurantRepo,
            IMapper Mapper
        )
        {
            _logger = logger;
            _restaurantRepo = restaurantRepo;
            _mapper = Mapper;
        }

        

        public async Task<int> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating a new Restaurant {@Restaurant}",request);
            var restaurant = _mapper.Map<Restaurant>(request);
            int id = await _restaurantRepo.Create(restaurant);

            return id;
        }
    }
}