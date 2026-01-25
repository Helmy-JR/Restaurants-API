using MediatR;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Restaurants.Queries.GetRestaurantByID
{
    public class GetRestaurantByIdQyery (int id): IRequest<RestaurantDto?>
    {
        public int Id { get; } = id;
    }
}