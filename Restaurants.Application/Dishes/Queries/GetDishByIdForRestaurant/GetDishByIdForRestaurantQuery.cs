using MediatR;
using Restaurants.Application.Dishes.Dtos;

namespace Restaurants.Application.Dishes.Queries.GetDishByIdForRestaurant
{
    public class GetDishByIdForRestaurantQuery: IRequest<DishDto>
    {
        public int RestaurantId { get; }
        public int DishId { get; }

        public GetDishByIdForRestaurantQuery(int restaurantId, int dishId)
        {
            RestaurantId = restaurantId;
            DishId = dishId;
        }
    }
}