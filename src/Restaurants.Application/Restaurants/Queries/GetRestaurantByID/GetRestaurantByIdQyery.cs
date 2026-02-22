using MediatR;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants.Queries.GetRestaurantByID
{
    public class GetRestaurantByIdQyery : IRequest<RestaurantDto>
    {
        public int Id { get; }

        public GetRestaurantByIdQyery(int id)
        {
            Id = id;
        }
    }
}