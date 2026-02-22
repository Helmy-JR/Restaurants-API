using MediatR;

namespace Restaurants.Application.Dishes.Commands.DeleteAllDishesForRestaunt
{
    public class DeleteAllDishesForRestaurantCommand : IRequest
    {
        public int RestaurantId { get; }
        public DeleteAllDishesForRestaurantCommand(int restaurantId)
        {
            RestaurantId = restaurantId;
        }
    }
}