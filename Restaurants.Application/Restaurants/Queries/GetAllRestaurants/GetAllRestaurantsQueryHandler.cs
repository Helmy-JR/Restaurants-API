using MediatR;
using Restaurants.Application.Restaurants.Dtos;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Repositories;


namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants
{
    public class GetAllRestaurantsQueryHandler : IRequestHandler<GetAllRestaurantsQuery, IEnumerable<RestaurantDto>>
    {
        private readonly ILogger<GetAllRestaurantsQuery> _logger ;
        private readonly IRestaurantRepository _restaurantRepo ;
        private readonly IMapper _mapper ;

        public GetAllRestaurantsQueryHandler(
            ILogger<GetAllRestaurantsQuery> logger,
            IRestaurantRepository restaurantRepo,
            IMapper Mapper
        )
        {
            _logger = logger;
            _restaurantRepo = restaurantRepo;
            _mapper = Mapper;
        }
        public async Task<IEnumerable<RestaurantDto>> Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching all Restaurants !!!");
            var restaurants = await _restaurantRepo.GetAllAsync();
            var restaurantDtos = _mapper.Map<IEnumerable<RestaurantDto>>(restaurants);

            return restaurantDtos;
        }
    }
}