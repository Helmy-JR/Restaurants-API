using MediatR;
using Restaurants.Application.Restaurants.Dtos;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Repositories;
using Restaurants.Application.Common;


namespace Restaurants.Application.Restaurants.Queries.GetAllRestaurants
{
    public class GetAllRestaurantsQueryHandler : IRequestHandler<GetAllRestaurantsQuery, PagedResult<RestaurantDto>>
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
        public async Task<PagedResult<RestaurantDto>> Handle(GetAllRestaurantsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching all Restaurants !!!");
            var (restaurants,totalCount) = await _restaurantRepo.GetAllMatchingAsync(request.SearchPhrase, 
                request.PageSize, request.PageNumber, request.SortBy, request.SortDirection);
            
            var restaurantDtos = _mapper.Map<IEnumerable<RestaurantDto>>(restaurants);

            var result = new PagedResult<RestaurantDto>(restaurantDtos, totalCount, request.PageSize, request.PageNumber);

            return result;
        }
    }
}