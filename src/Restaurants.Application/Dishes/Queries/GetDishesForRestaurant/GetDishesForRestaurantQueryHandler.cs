using MediatR;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Restaurants.Application.Dishes.Dtos;
using Restaurants.Domain.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Restaurants.Domain.Exceptions;

namespace Restaurants.Application.Dishes.Queries.GetDishesForRestaurant
{
    public class GetDishesForRestaurantQueryHandler : IRequestHandler<GetDishesForRestaurantQuery, IEnumerable<DishDto>>
    {
        private readonly ILogger<GetDishesForRestaurantQueryHandler> _logger;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly IMapper _mapper;

        public GetDishesForRestaurantQueryHandler(
            ILogger<GetDishesForRestaurantQueryHandler> logger,
            IRestaurantRepository restaurantRepository,
            IMapper mapper
        )
        {
            _logger = logger;
            _restaurantRepository = restaurantRepository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<DishDto>> Handle(GetDishesForRestaurantQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving dishes for Restaurant: {RestaurantId}", request.RestaurantId);
            var restaurant = await _restaurantRepository.GetByIdAsync(request.RestaurantId);
            if(restaurant == null)
            {
                throw new NotFoundException(nameof(restaurant), request.RestaurantId.ToString());
            }

            var results = _mapper.Map<IEnumerable<DishDto>>(restaurant.Dishes);
            return results;
        }
    }
}