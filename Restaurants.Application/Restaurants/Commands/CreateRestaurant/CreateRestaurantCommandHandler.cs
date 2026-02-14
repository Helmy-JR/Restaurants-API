using MediatR;
using Restaurants.Domain.Entities;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Repositories;
using Restaurants.Application.Restaurants.Dtos;
using Restaurants.Application.Users;

namespace Restaurants.Application.Restaurants.Commands.CreateRestaurant
{
    public class CreateRestaurantCommandHandler
     : IRequestHandler<CreateRestaurantCommand, RestaurantDto>
    {
        private readonly ILogger<CreateRestaurantCommandHandler> _logger ;
        private readonly IRestaurantRepository _restaurantRepo ;
        private readonly IMapper _mapper ;
        private readonly IUserContext _userContext;

        public CreateRestaurantCommandHandler(
            ILogger<CreateRestaurantCommandHandler> logger,
            IRestaurantRepository restaurantRepo,
            IMapper Mapper,
            IUserContext userContext
        )
        {
            _logger = logger;
            _restaurantRepo = restaurantRepo;
            _mapper = Mapper;
            _userContext = userContext;
        }

        

        public async Task<RestaurantDto> Handle(CreateRestaurantCommand request, CancellationToken cancellationToken)
        {
            var currUser = _userContext.GetCurrentUser();

            _logger.LogInformation("User: {UserEmail} with ID {UserID}Creating a new Restaurant {@Restaurant}",
                request,currUser.Email, currUser.Id
            );
            
            var restaurant = _mapper.Map<Restaurant>(request);

            restaurant.OwnerId = currUser.Id;
            
            await _restaurantRepo.Create(restaurant);

            return _mapper.Map<RestaurantDto>(restaurant);
        }
    }
}