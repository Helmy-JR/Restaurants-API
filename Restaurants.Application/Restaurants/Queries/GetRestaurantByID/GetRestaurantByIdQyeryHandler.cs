using MediatR;
using Restaurants.Application.Restaurants.Dtos;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Restaurants.Domain.Repositories;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Entities;


namespace Restaurants.Application.Restaurants.Queries.GetRestaurantByID
{
    public class GetRestaurantByIdQyeryHandler : IRequestHandler<GetRestaurantByIdQyery, RestaurantDto>
    {
        private readonly ILogger<GetRestaurantByIdQyery> _logger ;
        private readonly IRestaurantRepository _restaurantRepo ;
        private readonly IMapper _mapper ;

        public GetRestaurantByIdQyeryHandler(
            ILogger<GetRestaurantByIdQyery> logger,
            IRestaurantRepository restaurantRepo,
            IMapper Mapper
        )
        {
            _logger = logger;
            _restaurantRepo = restaurantRepo;
            _mapper = Mapper;
        }
        public async Task<RestaurantDto> Handle(GetRestaurantByIdQyery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching Restaurant by Id: {Id} !!!", request.Id);
            var restaurant = await _restaurantRepo.GetByIdAsync(request.Id)
                ?? throw new NotFoundException(nameof(Restaurant), request.Id.ToString());
            var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);

            return restaurantDto;
        }
    }
}